using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels.ObserverMessages;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * Pick Up View Model
	 * */

	public class PickUpViewModel 
		: MvxViewModel
    {

		private IMvxMessenger _messenger;
		private readonly IDataService _dataService;


		public PickUpViewModel (IDataService dataService, IMvxMessenger messenger)
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

		public void PublishData()
		{
			var pickUpData = new PickUpData () {
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
			_dataService.Add(pickUpData);
			_messenger.Publish (new PickUpDataMessage (this, pickUpData));
		}

	}
}
