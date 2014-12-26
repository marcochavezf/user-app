using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using System.Collections.Generic;
using Kangou.Core.ViewModels.ObserverMessages;
using System;
using System.Threading.Tasks;
using Kangou.Core.ViewModels;
using Cirrious.CrossCore.Platform;
using Kangou.Core.WebClients;

namespace Kangou.Core.ViewModels
{
	public class ActiveOrderListViewModel
		: MvxViewModel
    {
		private KangouClient _kangouClient;
		private UserData _userData;
		private IMvxMessenger _messenger;

		private bool _isBusy;
		public bool IsBusy
		{   
			get { return _isBusy; }
			set { 
				if (IsBusy != value) {
					_isBusy = value; 
					RaisePropertyChanged (() => IsBusy); 
				}
			}
		}
	
		public ActiveOrderListViewModel (IDataService dataService, IMvxMessenger messenger, IMvxJsonConverter _jsonConverter)
		{
			_kangouClient = new KangouClient (_jsonConverter);
			_userData = dataService.GetUserData ();
			_messenger = messenger;

			IsBusy = true;

			var instanceConnectionManager = ConnectionManager.Instance;
			ConnectionManager.FailedToConnect (()=>{
				ConnectionManager.Off(SocketEvents.Connected);
				ConnectionManager.Off(SocketEvents.ActiveOrder);

				Debug.WriteLine("FailedToConnect");
				PopulateListFromServer();
			});
		}

		public void PopulateListFromServer (){

			IsThereActiveOrders = true;

			if ( !String.IsNullOrWhiteSpace( ActiveOrder.LAST_ORDER_PLACED_ID )) {
				var orderId = ActiveOrder.LAST_ORDER_PLACED_ID;
				ActiveOrder.LAST_ORDER_PLACED_ID = "";
				OpenActiveOrder (orderId);
				return;
			}

			Task.Run (() => {
				_kangouClient.GetActiveOrderList (_userData.Email, 
					(activeOrderList) => {
						InvokeOnMainThread (delegate {
							ActiveOrderList = activeOrderList;
							IsBusy = false;
						});
					},
					(error) => {
						ActiveOrderList = new List<ActiveOrder> ();
						Debug.WriteLine ("error: {0}", error);
					});
			});
		}

		private bool _isTableVisible;
		public bool IsTableVisible
		{
			get { return _isTableVisible; }
			set
			{
				_isTableVisible = value; 
				RaisePropertyChanged(() => IsTableVisible);
			}
		}

		private bool _isThereActiveOrders;
		public bool IsThereActiveOrders { 
			get { return _isThereActiveOrders; }
			set {
				_isThereActiveOrders = value;
				RaisePropertyChanged (() => IsThereActiveOrders);
			}
		}

		private List<ActiveOrder> _activeOrderList;
		public List<ActiveOrder> ActiveOrderList
		{
			get { return _activeOrderList; }
			set { 
				_activeOrderList = value; 
				RaisePropertyChanged(() => ActiveOrderList);
				IsTableVisible = ActiveOrderList.Count > 0;
				IsThereActiveOrders = ActiveOrderList.Count > 0;
			}
		}

		private ICommand _selectActiveOrderCommand;
		public ICommand SelectActiveOrderCommand
		{
			get
			{
				_selectActiveOrderCommand = _selectActiveOrderCommand ?? new MvxCommand<ActiveOrder>(activeOrder => {
					OpenActiveOrder(activeOrder._id);
				});
				return _selectActiveOrderCommand;
			}
		}

		private void OpenActiveOrder(string orderId){
			InvokeOnMainThread (delegate {
				IsBusy = true;
			});

			ConnectionManager.ConnectionState = ConnectionStates.USER_WANTS_TO_BE_CONNECTED;
			var connectionState = ConnectionManager.ConnectionState;

			Debug.WriteLine (ConnectionManager.ConnectionState);
			ConnectionManager.Connect(_userData.Id.ToString());

			ConnectionManager.On(SocketEvents.Connected, (data) => {
				ConnectionManager.Off(SocketEvents.Connected);

				Debug.WriteLine ("On Connected state: {0}", connectionState);

				if(connectionState != ConnectionStates.USER_WANTS_TO_BE_CONNECTED){
					Debug.WriteLine ("Ignored because of state: {0}", connectionState);
					return;
				}

				System.Diagnostics.Debug.WriteLine ("connected On: {0}", data["isSuccesful"] );
				ConnectionManager.Emit( SocketEvents.ActiveOrder, ConnectionManager.OrderIdJsonString(orderId));
				ConnectionManager.ConnectionState = ConnectionStates.CONNECTED_BY_SERVER;
				connectionState = ConnectionManager.ConnectionState;
			});

			ConnectionManager.On (SocketEvents.ActiveOrder, (data) => {
				ConnectionManager.Off(SocketEvents.ActiveOrder);

				if(IsBusy){
					Debug.WriteLine ("Opening On StatusOrderViewModel" );

					ShowViewModel<StatusOrderViewModel>(new ActiveOrder(){
						_id = data["_id"].ToString(),
						Status = data["status"].ToString(),
						Date = data["date"].ToString(),
						PickUpLat = Convert.ToDouble( data["pickup"]["lat"].ToString() ),
						PickUpLng = Convert.ToDouble( data["pickup"]["lng"].ToString() ),
						DropOffLat = Convert.ToDouble( data["dropoff"]["lat"].ToString() ),
						DropOffLng = Convert.ToDouble( data["dropoff"]["lng"].ToString() )
					});
					InvokeOnMainThread (delegate {
						ActiveOrderList = new List<ActiveOrder> ();
						IsBusy = false;
					});
				}
			});
		}

		public void PublishMessageViewOpened()
		{
			_messenger.Publish<ChangeStateViewToggledMessage> (new ChangeStateViewToggledMessage(this, TypeRootViewOpened.ACTIVE_ORDER_LIST));
		}
	}
}