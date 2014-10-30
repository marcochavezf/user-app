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

		public ItemsViewModel (IMvxMessenger messenger)
		{
			_messenger = messenger;
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

		public void PublishData()
		{
			var itemsData = new ItemsData () {
				Items = this.Items
			};
			_messenger.Publish (new ItemsDataMessage (this, itemsData));
		}
	}
}
