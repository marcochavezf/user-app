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

		private MvxSubscriptionToken _tokenPickUpAddress;
		private PickUpData pickUpData;

		public ItemsViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_messenger = messenger;
			_dataService = dataService;
			_tokenPickUpAddress = messenger.Subscribe<PickUpDataMessage> (UpdatePickUpViewModel);
		}

		private void UpdatePickUpViewModel(PickUpDataMessage pickUpDataMessage){
			pickUpData = pickUpDataMessage.PickUpData;
			PickUpAddress = pickUpData.Address;
		}

		/* * * * * * * * * * * *
		 * Items Property
		 */

		private string _items = "";
		public string Items { 
			get { return _items; }
			set {
				_items = value;
				RaisePropertyChanged (() => Items);
			}
		}

		/* * * * * * * * * * * *
		 * Pick Up Address Property
		 */

		private string _pickUpAddress = "Agregar dirección (Opcional)";
		public string PickUpAddress { 
			get { return _pickUpAddress; }
			set {
				_pickUpAddress = value;
				RaisePropertyChanged (() => PickUpAddress);
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Pick Up Address Command
		 */

		private MvxCommand _addPickUpAddressCommand;
		public ICommand AddPickUpAddressCommand {
			get {
				_addPickUpAddressCommand = _addPickUpAddressCommand ?? new MvxCommand (()=>
					{
						if (_dataService.CountPickUpData > 0) 
							ShowViewModel <PickUpListViewModel> ();
						else
							ShowViewModel <PickUpViewModel> ();

					});
				return _addPickUpAddressCommand;
			}
		}

		public void PublishData()
		{
			var itemsData = new ItemsData () {
				Items = this.Items,
				PickUpData = pickUpData
			};
			_messenger.Publish (new ItemsDataMessage (this, itemsData));
		}
	}
}
