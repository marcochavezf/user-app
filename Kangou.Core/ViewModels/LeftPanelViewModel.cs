using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Kangou.Core;

namespace Kangou.Core.ViewModels
{
	public class LeftPanelViewModel : MvxViewModel
    {

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

