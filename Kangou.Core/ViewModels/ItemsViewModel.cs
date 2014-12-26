using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels.ObserverMessages;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * Items View Model
	 * */

	public class ItemsViewModel 
		: MvxViewModel
    {
		private readonly IMvxMessenger _messenger;
		private readonly IDataService _dataService;

		public ItemsViewModel (IMvxMessenger messenger, IDataService dataService)
		{
			_messenger = messenger;
			_dataService = dataService;
		}

		/* * * * * * * * * * * *
		 * Items Property
		 */

		private string _items;
		public string Items { 
			get { return _items; }
			set {
				_items = value; RaisePropertyChanged (() => Items);
			}
		}

		public static bool _isAPurchase;
		public bool IsAPurchase { 
			get { return _isAPurchase; }
			set {
				_isAPurchase = value;
				RaisePropertyChanged (() => IsAPurchase);
			}
		}

		public void PublishData()
		{
			var itemsData = new ItemsData () {
				Items = this.Items,
				IsAPurchase = this.IsAPurchase
			};
			_messenger.Publish (new ItemsDataMessage (this, itemsData));
			if (RegisterOrderViewModel.isStraightNavigation) {
				if (_dataService.CountPickUpData > 0)
					ShowViewModel <PickUpListViewModel> ();
				else
					ShowViewModel <PickUpViewModel> ();
			} else {
				Close (this);
			}
		}
	}
}
