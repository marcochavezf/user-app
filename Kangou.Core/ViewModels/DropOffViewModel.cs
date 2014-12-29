using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels.ObserverMessages;
using System;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * Drop Off View Model
	 * */

	public class DropOffViewModel 
		: MvxViewModel
    {

		private readonly IMvxMessenger _messenger;
		private readonly IDataService _dataService;
		private MvxSubscriptionToken _subscriptionId;

		public DropOffViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_messenger = messenger;
			_dataService = dataService;
		}

		/* * * * * * * * * * * *
		 * Full Name Property
		 */

		private string _fullName;
		public string FullName { 
			get { return _fullName; }
			set {
				_fullName = value;
				RaisePropertyChanged (() => FullName);
			}
		}

		/* * * * * * * * * * * *
		* References Property
		*/

		private string _references;
		public string References { 
			get { return _references; }
			set {
				_references = value;
				RaisePropertyChanged (() => References);
			}
		}
	
		/* * * * * * * * * * * *
		* Short Address Property
		*/

		private string _addressToDisplay;
		public string AddressToDisplay { 
			get { return _addressToDisplay; }
			set {
				_addressToDisplay = value;
				RaisePropertyChanged (() => AddressToDisplay);
			}
		}

		/* * * * * * * * * * * *
		* Address Property
		*/

		private string _street;
		public string Street { 
			get { return _street; }
			set {
				_street = value;
				RaisePropertyChanged (() => Street);
			}
		}

		private string _subLocality;
		public string SubLocality { 
			get { return _subLocality; }
			set {
				_subLocality = value;
				RaisePropertyChanged (() => SubLocality);
			}
		}

		private string _locality;
		public string Locality { 
			get { return _locality; }
			set {
				_locality = value;
				RaisePropertyChanged (() => Locality);
			}
		}

		private string _administrativeArea;
		public string AdministrativeArea { 
			get { return _administrativeArea; }
			set {
				_administrativeArea = value;
				RaisePropertyChanged (() => AdministrativeArea);
			}
		}

		private string _country;
		public string Country { 
			get { return _country; }
			set {
				_country = value;
				RaisePropertyChanged (() => Country);
			}
		}

		private string _postalCode;
		public string PostalCode { 
			get { return _postalCode; }
			set {
				_postalCode = value;
				RaisePropertyChanged (() => PostalCode);
			}
		}

		private string _isoCountryCode;
		public string IsoCountryCode { 
			get { return _isoCountryCode; }
			set {
				_isoCountryCode = value;
				RaisePropertyChanged (() => IsoCountryCode);
			}
		}

		private double _lat;
		public double Lat { 
			get { return _lat; }
			set {
				_lat = value;
				RaisePropertyChanged (() => Lat);
			}
		}

		private double _lng;
		public double Lng { 
			get { return _lng; }
			set {
				_lng = value;
				RaisePropertyChanged (() => Lng);
			}
		}

		private MvxCommand _openPlaceAutocompleteCommand;
		public ICommand OpenPlaceAutocompleteCommand {
			get {
				_openPlaceAutocompleteCommand = _openPlaceAutocompleteCommand ?? new MvxCommand (DoOpenPlaceAutocompleteCommand);
				return _openPlaceAutocompleteCommand;
			}
		}

		private void DoOpenPlaceAutocompleteCommand ()
		{
			_subscriptionId = _messenger.Subscribe<PlaceDetailsResponseMessage> ((placeDetailsResponseMessage)=>{
				ChangeLocation(placeDetailsResponseMessage.PlaceDetailsResponse);
				_messenger.Unsubscribe<PlaceDetailsResponseMessage>(_subscriptionId);
			});
			ShowViewModel<PlaceAutocompleteViewModel> ();   
		}

		public void PublishData()
		{
			var dropOffData = new DropOffData () {
				FullName =  this.FullName,
				AddressToDisplay = this.AddressToDisplay,
				Street = this.Street,
				SubLocality = this.SubLocality,
				Locality = this.Locality,
				AdministrativeArea = this.AdministrativeArea,
				Country = this.Country,
				PostalCode = this.PostalCode,
				IsoCountryCode = this.IsoCountryCode,
				Lat = this.Lat,
				Lng = this.Lng,
				References = this.References
			};
			_dataService.Add(dropOffData);
			_messenger.Publish (new DropOffDataMessage (this, dropOffData));

			if(RegisterOrderViewModel.isStraightNavigation){
				if (_dataService.CountCreditCardData > 0)
					ShowViewModel <CreditCardListViewModel> ();
				else
					ShowViewModel <CreditCardViewModel> ();
			} else {
				Close(this);
			}
		}

		public Action<PlaceDetailsResponse> ChangeLocation { get; set; }

	}
}
