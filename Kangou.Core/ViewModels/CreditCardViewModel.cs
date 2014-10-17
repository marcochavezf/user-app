using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Text.RegularExpressions;
using Kangou.Core.Services.DataStore;
using Kangou.Core.ViewModels.ObserverMessages;
using Cirrious.CrossCore.Platform;
using Kangou.Core.WebClients;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using Kangou.Core.Helpers;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * Credit Card View Model
	 * */

	public class CreditCardViewModel 
		: MvxViewModel
    {

		private IMvxMessenger _messenger;
		public DropOffData DropOffData { get; set; }

		private readonly IDataService _dataService;
		private readonly ConektaClient _conektaClient;


		public CreditCardViewModel (IMvxJsonConverter jsonConverter, IDataService dataService, IMvxMessenger messenger)
		{
			_messenger = messenger;
			_dataService = dataService;
			_conektaClient = new ConektaClient(jsonConverter);
		}

		private void UpdateDropOffData(DropOffDataMessage dropOffDataMessage){
			DropOffData = dropOffDataMessage.DropOffData;
		}


		/* * * * * * * * * * * *
		 * Credit Card Properties
		 */

		private string _creditCardNumber;
		public string CreditCardNumber {
			get { return _creditCardNumber; }
			set {
				_creditCardNumber = value;
				RaisePropertyChanged (() => CreditCardNumber);
			}
		}

		private string _expirationDate;
		public string ExpirationDate { 
			get { return _expirationDate; }
			set {
				_expirationDate = value;
				RaisePropertyChanged (() => ExpirationDate);
			}
		}

		private string _cvv;
		public string CVV {
			get { return _cvv; }
			set {
				_cvv = value;
				RaisePropertyChanged (() => CVV);
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{   
			get { return _isBusy; }
			set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
		}

		public bool PublishData()
		{
			/*
			var name = "Marco";
			var email = "marco@kangou.mx";
			var phoneNumber = "5512345678";
			var address = "Dirección prueba";
			var cardNumber = "4766 8400 7383 1823";
			var expMonth = "02";
			var expYear = "2020";
			var cvc = "693";

			var client = _conektaClient.CreateClient (
				             name,
				             email,
				             phoneNumber,
				             address,
				             cardNumber,
				             expMonth,
				             expYear,
				             cvc
			             );
			*/

			//Acquiring DropOff Data
			_messenger.Publish (new RequestDropOffInfoMessage (this));
			var userData = _dataService.GetUserData ();
			var name = userData.Name;
			var email = userData.Email;
			var phoneNumber = userData.PhoneNumber;
			var address = DropOffData.Street;

			string [] expDate = this.ExpirationDate.Split(new Char [] {'/'});

			var cardNumber = this.CreditCardNumber;
			var expMonth = expDate [0];
			var expYear = expDate [1];
			var cvc = this.CVV;

			//Creating Conketa Client
			var client = _conektaClient.CreateClient(
				name,
				email,
				phoneNumber,
				address,
				cardNumber,
				expMonth,
				expYear,
				cvc
			);


			//It wasn't a succesful operation to create client
			if (client.default_card_id == null)
				return false;
			var creditCardNumber = this.CreditCardNumber;
			string pattern = @"^[0-9]+$";
			Regex regex = new Regex(pattern);
			if (regex.IsMatch (creditCardNumber [0].ToString ())) 
				creditCardNumber = StringFormater.HideCreditCardNumber (creditCardNumber);
		
			var creditCardData = new CreditCardData () {
				CreditCardNumber = creditCardNumber,
				CardId = client.default_card_id,
				TypeCardId = "Conekta"
			};
			_dataService.Add(creditCardData);
			_messenger.Publish (new CreditCardDataMessage (this, creditCardData));
			//All was succesfull
			return true;
		}

	}
}