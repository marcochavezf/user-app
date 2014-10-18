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

namespace Kangou.Core
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

				for(int i=0; i<1000; i++)
					Debug.WriteLine(i);

				InvokeOnMainThread (delegate {  

					var activeOrderList = new List<ActiveOrder> ();
					activeOrderList.Add (new ActiveOrder (){ Id = 234, Date = DateTime.Now });
					activeOrderList.Add (new ActiveOrder (){ Id = 235, Date = DateTime.Now });
					ActiveOrderList = activeOrderList;

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

		public ICommand SelectDataCommand
		{
			get
			{
				return new MvxCommand<ActiveOrder>(activeOrder => {
					Debug.WriteLine("Active order: {0}",activeOrder);
				});
			}
		}

	}
}
