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
using KangouMessenger.Core;
using Cirrious.CrossCore.Platform;
using Kangou.Core.WebClients;

namespace Kangou.Core.ViewModels
{
	public class ActiveOrderListViewModel
		: MvxViewModel
    {
		private KangouClient _kangouClient;
		private UserData _userData;

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

			IsBusy = true;

			ConnectionManager.FailedToConnect (()=>{
				Debug.WriteLine("FailedToConnect");
				InvokeOnMainThread (delegate {
					IsBusy = false;
				});
			});
		}

		public void PopulateListFromServer (){

			if (ActiveOrder.LAST_ORDER_PLACED_ID > 0) {
				var orderId = ActiveOrder.LAST_ORDER_PLACED_ID;
				ActiveOrder.LAST_ORDER_PLACED_ID = -1;
				OpenActiveOrder (orderId);
				return;
			}

			Task.Run (() => {
				_kangouClient.GetActiveOrderList (_userData.Id, 
					(activeOrderList) => {
						ActiveOrderList = activeOrderList;
						IsBusy = false;
					},
					(error) => {
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
					OpenActiveOrder(activeOrder.Id);
				});
			}
		}

		private void OpenActiveOrder(int orderId){
			IsBusy = true;
			Task.Run (()=>{
				System.Diagnostics.Debug.WriteLine ("ConnectAsync");
				ConnectionManager.Connect();
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
						Id = Convert.ToInt32(data["id"].ToString()),
						Status = data["status"].ToString(),
						Date = data["date"].ToString()
					});
					InvokeOnMainThread (delegate {
						IsBusy = false;
					});
				}
			});
		}
	}
}
