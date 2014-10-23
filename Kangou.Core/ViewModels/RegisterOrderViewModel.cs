using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using Kangou.Core.Services.DataSender;
using Kangou.Core.ViewModels.ObserverMessages;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using Kangou.Core.WebClients;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Kangou.Core.Helpers;
using System.Threading.Tasks;


namespace Kangou.Core.ViewModels
{
    public class RegisterOrderViewModel 
		: MvxViewModel
    {
		public static Location LOCATION = new Location ();

		private readonly MvxSubscriptionToken 
		_tokenItems,
		_tokenPickUpAddress,
		_tokenDropOffData,
		_tokenCreditCardNumber,
		_tokenPromoCode,
		_tokenResetOrderData,
		_tokenRequestDropOffInfo;
		private readonly IDataService _dataService;
		private readonly IMvxMessenger _messenger;

		private readonly KangouClient _kangouClient;

		private ItemsData _itemsData;
		private PickUpData _pickUpData;
		private DropOffData _dropOffData;
		private CreditCardData _creditCardData;

		public RegisterOrderViewModel (IMvxJsonConverter _jsonConverter, ILocationService locationService, IDataService dataService, IMvxMessenger messenger)
		{
			ResetOrderData (null);

			_kangouClient = new KangouClient (_jsonConverter);

			_dataService = dataService;
			_messenger = messenger;
			_tokenItems = messenger.Subscribe<ItemsDataMessage> (UpdateItems);
			_tokenPickUpAddress = messenger.Subscribe<PickUpDataMessage> (UpdatePickUpViewModel);
			_tokenDropOffData = messenger.Subscribe<DropOffDataMessage> (UpdateDropOffData);
			_tokenCreditCardNumber = messenger.Subscribe<CreditCardDataMessage> (UpdateCreditCardData);
			_tokenPromoCode = messenger.Subscribe<PromoCodeMessage> (UpdatePromoCode);
			_tokenResetOrderData = messenger.Subscribe<ResetOrderDataMessage> (ResetOrderData);
			_tokenRequestDropOffInfo = messenger.Subscribe<RequestDropOffInfoMessage> (RequestDropOffData);
		}

		private void RequestDropOffData(RequestDropOffInfoMessage requestDropOffInfoMessage){
			var creaditCardViewModel = (CreditCardViewModel)requestDropOffInfoMessage.Sender;
			creaditCardViewModel.DropOffData = _dropOffData;
		}

		private void ResetOrderData(ResetOrderDataMessage resetOrderDataMessage)
		{
			_itemsData = null;
			_pickUpData = null;
			_dropOffData = null;
			_creditCardData = null;

			Items = "Agregar productos";
			PickUpAddress = "Seleccionar una dirección";
			DropOffAddress = "Seleccionar una dirección";
			InfoPaymentMethod = "Seleccionar forma de pago";
			PromoCode = "Código de promoción";
		}

		private void UpdateItems(ItemsDataMessage itemsDataMessage){

			_itemsData = itemsDataMessage.ItemsData;

			var itemsInfoString = _itemsData.Items;
			if (itemsInfoString.Equals (""))
				return;

			Items = itemsInfoString;
			EnablePickUpButton ();
		}
			
		private void UpdatePickUpViewModel(PickUpDataMessage pickUpDataMessage){
			_pickUpData = pickUpDataMessage.PickUpData;
			PickUpAddress = _pickUpData.AddressToDisplay;
			EnableDropOffButton ();
		}

		private void UpdateDropOffData(DropOffDataMessage dropOffDataMessage){
			_dropOffData = dropOffDataMessage.DropOffData;
			DropOffAddress = _dropOffData.AddressToDisplay;
			EnablePaymentMethodButton ();
		}

		private void UpdateCreditCardData(CreditCardDataMessage creditCardMessage){
			_creditCardData = creditCardMessage.CreditCardData;
			var creditCardString = _creditCardData.CreditCardNumber.ToString ();
			InfoPaymentMethod = creditCardString;
			EnablePUKButton ();
		}

		private void UpdatePromoCode(PromoCodeMessage promoCode){
			PromoCode = promoCode.PromoCode;
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Properties
		 */

		public Action EnablePickUpButton { get; set; }
		public Action EnableDropOffButton { get; set; }
		public Action EnablePaymentMethodButton { get; set; }
		public Action EnablePUKButton { get; set; }
		public Action DisableButtons { get; set; }

		private string _items;
		public string Items { 
			get { return _items; }
			set {
				_items = value;
				RaisePropertyChanged (() => Items);
			}
		}

		private string _pickUpAddress;
		public string PickUpAddress { 
			get { return _pickUpAddress; }
			set {
				_pickUpAddress = value;
				RaisePropertyChanged (() => PickUpAddress);
			}
		}

		private string _dropOffAddress;
		public string DropOffAddress { 
			get { return _dropOffAddress; }
			set {
				_dropOffAddress = value;
				RaisePropertyChanged (() => DropOffAddress);
			}
		}
			
		private string _infoPaymentMethod;
		public string InfoPaymentMethod { 
			get { return _infoPaymentMethod; }
			set {
				_infoPaymentMethod = value;
				RaisePropertyChanged (() => InfoPaymentMethod);
			}
		}

		private string _promoCode;
		public string PromoCode { 
			get { return _promoCode; }
			set {
				_promoCode = value;
				RaisePropertyChanged (() => PromoCode);
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Items
		 */

		private MvxCommand _addItemsCommand;
		public ICommand AddItemsCommand {
			get {
				_addItemsCommand = _addItemsCommand ?? new MvxCommand (()=>
					{
						ShowViewModel <ItemsViewModel> ();
					});
				return _addItemsCommand;
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Pick Up
		 */

		private MvxCommand _addPickUpAddressCommand;
		public ICommand AddPickUpAddressCommand {
			get {
				_addPickUpAddressCommand = _addPickUpAddressCommand ?? new MvxCommand (()=>
					{
						if (_itemsData == null)
							return;

						if (_dataService.CountPickUpData > 0) 
							ShowViewModel <PickUpListViewModel> ();
						else
							ShowViewModel <PickUpViewModel> ();
					});
				return _addPickUpAddressCommand;
			}
		}


		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Drop Off Address
		 */

		private MvxCommand _addDropOffAddressCommand;
		public ICommand AddDropOffAddressCommand {
			get {
				_addDropOffAddressCommand = _addDropOffAddressCommand ?? new MvxCommand (()=>
					{
						if (_pickUpData == null)
							return;

						if (_dataService.CountDropOffData > 0) 
							ShowViewModel <DropOffListViewModel> ();
						else
							ShowViewModel <DropOffViewModel> ();
					});
				return _addDropOffAddressCommand;
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Select Payment Method
		 */

		private MvxCommand _addCreditCardCommand;
		public ICommand AddCreditCardCommand {
			get {
				_addCreditCardCommand = _addCreditCardCommand ?? new MvxCommand (()=>
					{
						if (_dataService.CountCreditCardData > 0) 
							ShowViewModel <CreditCardListViewModel> ();
						else
							ShowViewModel <CreditCardViewModel> ();
					});
				return _addCreditCardCommand;
			}
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Add Promo Code
		 */

		private MvxCommand _addPromoCodeCommand;
		public ICommand AddPromoCodeCommand {
			get {
				_addPromoCodeCommand = _addPromoCodeCommand ?? new MvxCommand (()=>
					{
						ShowViewModel <PromoCodeViewModel> ();
					});
				return _addPromoCodeCommand;
			}
		}


		public bool IsDeliveryDataSet()
		{
			if (_itemsData == null) {
				AddItemsCommand.Execute (null);
				return false;
			}

			if (_dropOffData == null) {
				AddDropOffAddressCommand.Execute (null);
				return false;
			}

			return true;
		}

		public bool IsPaymentInfoSet()
		{
			return !_infoPaymentMethod.Equals("Seleccionar forma de pago");
		}

		public void SetCashPaymentMethod()
		{
			_creditCardData = null;
			InfoPaymentMethod = "Efectivo";
			EnablePUKButton ();
		}

		public void ConfirmOrder(Action<int> successAction, Action<string> errorAction)
		{
			var userData = _dataService.GetUserData ();

			_kangouClient.SendOrderData(_itemsData, _pickUpData, _dropOffData, _creditCardData, userData, 
				(response) =>
				{
					ActiveOrder.LAST_ORDER_PLACED_ID = response;
					successAction(ActiveOrder.LAST_ORDER_PLACED_ID);
					ShowViewModel <ActiveOrderListViewModel> ();
					ResetOrderData (null);
					DisableButtons ();
				},
				(error) =>
				{
					errorAction(StringMessages.ERROR_ORDER_RESPONSE_MESSAGE);
				});
		}
	}
}
