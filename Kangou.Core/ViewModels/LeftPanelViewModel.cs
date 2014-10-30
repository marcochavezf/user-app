using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Kangou.Core;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Diagnostics;

namespace Kangou.Core.ViewModels
{
	public class LeftPanelViewModel : MvxViewModel
    {
		private readonly MvxSubscriptionToken _tokenMessenger;

		public LeftPanelViewModel (IMvxMessenger messenger)
		{
			_tokenMessenger = messenger.Subscribe<ChangeStateViewToggledMessage> (ChangeStateViewToggledMessage);
		}

		private void ChangeStateViewToggledMessage(ChangeStateViewToggledMessage requestDropOffInfoMessage){
			TogglePanelChanged (requestDropOffInfoMessage.TypeViewOpened);
		}
			
		public event Action<TypeRootViewOpened> TogglePanelChanged = (data) => {};

		public ICommand DoOpenRegisterOrder
		{
			get
			{
				return new MvxCommand(NavigateToRegisterOrder);
			}
		}

		void NavigateToRegisterOrder ()
		{
			ShowViewModel(typeof(RegisterOrderViewModel));
		}


		public ICommand DoOpenActiveOrdersList
		{
			get
			{
				return new MvxCommand(NavigateToActiveOrdersList);
			}
		}

		void NavigateToActiveOrdersList ()
		{
			ShowViewModel(typeof(ActiveOrderListViewModel));
		}


		public ICommand DoOpenEditProfile
		{
			get
			{
				return new MvxCommand(NavigateToEditProfile);
			}
		}

		void NavigateToEditProfile ()
		{
			ShowViewModel(typeof(EditProfileViewModel));
		}

    }
}

