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

			ConnectionManager.FailedToConnect (()=>{
				Debug.WriteLine("FailedToConnect");
				InvokeOnMainThread (delegate {
					IsBusy = false;
				});
			});
		}

		public void PopulateListFromServer (){

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

		private List<ActiveOrder> _activeOrderList;
		public List<ActiveOrder> ActiveOrderList
		{
			get { return _activeOrderList; }
			set { _activeOrderList = value; RaisePropertyChanged(() => ActiveOrderList); }
		}

		public ICommand SelectActiveOrderCommand
		{
			get
			{
				return new MvxCommand<ActiveOrder>(activeOrder => {
					OpenActiveOrder(activeOrder._id);
				});
			}
		}

		private void OpenActiveOrder(string orderId){
			IsBusy = true;
			Task.Run (()=>{
				System.Diagnostics.Debug.WriteLine ("ConnectAsync");
				ConnectionManager.Connect(_userData.Id.ToString());
			});

			ConnectionManager.On(SocketEvents.Connected, (data) => {
				ConnectionManager.Off(SocketEvents.Connected);
				System.Diagnostics.Debug.WriteLine ("connected On: {0}", data["isSuccesful"] );
				ConnectionManager.Emit( SocketEvents.ActiveOrder, ConnectionManager.OrderIdJsonString(orderId));
			});

			ConnectionManager.On (SocketEvents.ActiveOrder, (data) => {
				ConnectionManager.Off (SocketEvents.ActiveOrder);
				Debug.WriteLine ("ActiveOrder On: {0}", data["status"] );
				ConnectionManager.Off(SocketEvents.ActiveOrder);
				if(IsBusy){
					Debug.WriteLine ("Opening On StatusOrderViewModel" );

					ShowViewModel<StatusOrderViewModel>(new ActiveOrder(){
						_id = data["id"].ToString(),
						Status = data["status"].ToString(),
						Date = data["date"].ToString()
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