using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Kangou.Core.Services.DataStore;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Kangou.Core.ViewModels
{
	public class EditProfileViewModel : MvxViewModel
	{
		private readonly IDataService _dataService;
		private IMvxMessenger _messenger;

		public EditProfileViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_dataService = dataService;
			_messenger = messenger;

			UserData userData = _dataService.GetUserData ();
			if (userData != null) {
				Name = userData.Name;
				PhoneNumber = userData.PhoneNumber;
				Email = userData.Email;
			}
		}

		/* * * * * * * * * * * *
		 * Full Name Property
		 */

		private string _name;
		public string Name { 
			get { return _name; }
			set {
				_name = value;
				RaisePropertyChanged (() => Name);
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
			
		public void SaveData()
		{
			var userData = new UserData () {
				Name =  this.Name,
				PhoneNumber =  this.PhoneNumber,
				Email =  this.Email
			};
			_dataService.AddOrUpdate(userData);
		}

		public void PublishMessageViewOpened()
		{
			_messenger.Publish<ChangeStateViewToggledMessage> (new ChangeStateViewToggledMessage(this, TypeRootViewOpened.EDIT_PROFILE));
		}

	}
}

