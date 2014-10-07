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
			var pickUpData = new PickUpData () {
				FullName =  this.FullName,
				Address = this.Address,
				References = this.References
			};
			_dataService.Add(pickUpData);
			_messenger.Publish (new PickUpDataMessage (this, pickUpData));
		}

	}
}
