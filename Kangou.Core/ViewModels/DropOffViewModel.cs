using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels.ObserverMessages;

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
		 * Phone Number Property
		 */

		private string _phoneNumber;
		public string PhoneNumber { 
			get { return _phoneNumber; }
			set {
				_phoneNumber = value;
				RaisePropertyChanged (() => PhoneNumber);
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
		* Business Name Property
		*/

		private string _email;
		public string Email { 
			get { return _email; }
			set {
				_email = value;
				RaisePropertyChanged (() => Email);
			}
		}

		/* * * * * * * * * * * *
		* Address Property
		*/

		private string _address;
		public string Address { 
			get { return _address; }
			set {
				_address = value;
				RaisePropertyChanged (() => Address);
			}
		}

		public void PublishData()
		{
			var dropOffData = new DropOffData () {
				FullName =  this.FullName,
				PhoneNumber =  this.PhoneNumber,
				Email =  this.Email,
				Address = this.Address,
				References = this.References
			};
			_dataService.Add(dropOffData);
			_messenger.Publish (new DropOffDataMessage (this, dropOffData));
		}

	}
}
