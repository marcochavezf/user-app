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

namespace Kangou.Core.ViewModels
{
	public class ActiveOrderListViewModel
		: MvxViewModel
    {
		private bool _isBusy;
		public bool IsBusy
		{   
			get { return _isBusy; }
			set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
		}
	
		public ActiveOrderListViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			IsBusy = true;
			Task.Run (() => {

				for(int i=0; i<500; i++)
					Debug.WriteLine(i);

				InvokeOnMainThread (delegate {  

					var activeOrderList = new List<ActiveOrder> ();
					activeOrderList.Add (new ActiveOrder (){ Id = 234, Date = DateTime.Now });
					activeOrderList.Add (new ActiveOrder (){ Id = 235, Date = DateTime.Now });
					ActiveOrderList = activeOrderList;

					IsBusy = false;
				});
			});

			ConnectionManager.FailedToConnect (()=>{
				Debug.WriteLine("FailedToConnect");
				InvokeOnMainThread (delegate {
					IsBusy = false;
				});
			});
		}

		private List<ActiveOrder> _activeOrderList;
		public List<ActiveOrder> ActiveOrderList
		{
			get { return _activeOrderList; }
			set { _activeOrderList = value; RaisePropertyChanged(() => ActiveOrderList); }
		}

		private MvxCommand<ActiveOrder> _selectActiveOrder;
		public ICommand SelectActiveOrderCommand
		{
			get
			{
				_selectActiveOrder = _selectActiveOrder ?? new MvxCommand<ActiveOrder>(activeOrder => {
					IsBusy = true;
					Task.Run (()=>{
						System.Diagnostics.Debug.WriteLine ("ConnectAsync");
						ConnectionManager.Connect();
					});

					ConnectionManager.On(SocketEvents.Connected, (data) => {
						ConnectionManager.Off(SocketEvents.Connected);
						System.Diagnostics.Debug.WriteLine ("connected On: {0}", data["isSuccesful"] );
						ConnectionManager.Emit( SocketEvents.ActiveOrder, ConnectionManager.OrderIdJsonString(activeOrder.Id));
					});

					ConnectionManager.On (SocketEvents.ActiveOrder, (data) => {
						Debug.WriteLine ("ActiveOrder On: {0}", data["status"] );
						ConnectionManager.Off(SocketEvents.ActiveOrder);
						if(IsBusy){
							InvokeOnMainThread (delegate {
								IsBusy = false;
							});
							Debug.WriteLine ("Opening On StatusOrderViewModel" );
							//TODO if it's succesful and order is active, then open Status Order View
							ShowViewModel<StatusOrderViewModel>(activeOrder);
						}
					});
				});
				return _selectActiveOrder;
			}
		}
	}
}
