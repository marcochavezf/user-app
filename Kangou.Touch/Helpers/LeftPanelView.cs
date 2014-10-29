using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using System;
using Kangou.Core.ViewModels;
using SlidingPanels.Lib.PanelContainers;

namespace Kangou.Touch
{
	public partial class LeftPanelView : MvxViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

		private enum TypeViewOpened
		{
			REGISTER_ORDER, ACTIVE_ORDER_LIST, EDIT_PROFILE, NONE
		}

		private TypeViewOpened typeViewOpened;

        public override void ViewDidLoad ()
        {
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;

			View = new UIView (){ BackgroundColor = UIColor.White };
			View.Frame = new RectangleF (0, 0, 250, HEIGHT);
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

			base.ViewDidLoad();
			var viewModel = ViewModel as LeftPanelViewModel;
			typeViewOpened = TypeViewOpened.NONE;

			var widthButton = WIDTH;
			var heightButton = HEIGHT * 0.1f;
			var posXbutton = -1f;
			var offsetYbutton = HEIGHT * 0.08f;

			var showRegisterOrderViewButton = new UIButton (UIButtonType.RoundedRect);
			var showActiveOrdersViewButton = new UIButton (UIButtonType.RoundedRect);
			var showEditProfileViewButton = new UIButton (UIButtonType.RoundedRect);

			showRegisterOrderViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			showRegisterOrderViewButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			showRegisterOrderViewButton.TintColor = Constants.TINT_COLOR;
			showRegisterOrderViewButton.SetTitle ("      Crear Orden", UIControlState.Normal);
			showRegisterOrderViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showRegisterOrderViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showRegisterOrderViewButton.Layer.BorderWidth = 0.5f;
			showRegisterOrderViewButton.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.7f);
			showRegisterOrderViewButton.TouchUpInside += delegate {
				viewModel.DoOpenRegisterOrder.Execute(null);
				typeViewOpened = TypeViewOpened.REGISTER_ORDER;
				showRegisterOrderViewButton.TintColor = Constants.TINT_COLOR;
				showActiveOrdersViewButton.TintColor = UIColor.Gray;
				showEditProfileViewButton.TintColor = UIColor.Gray;
			};
			Add (showRegisterOrderViewButton);

			offsetYbutton += heightButton - 0.5f;

			showActiveOrdersViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			showActiveOrdersViewButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			showActiveOrdersViewButton.TintColor = UIColor.Gray;
			showActiveOrdersViewButton.SetTitle ("      Órdenes Activas", UIControlState.Normal);
			showActiveOrdersViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showActiveOrdersViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showActiveOrdersViewButton.Layer.BorderWidth = 0.5f;
			showActiveOrdersViewButton.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.7f);
			showActiveOrdersViewButton.TouchUpInside += delegate {
				viewModel.DoOpenActiveOrdersList.Execute(null);
				typeViewOpened = TypeViewOpened.ACTIVE_ORDER_LIST;
				showRegisterOrderViewButton.TintColor = UIColor.Gray;
				showActiveOrdersViewButton.TintColor = Constants.TINT_COLOR;
				showEditProfileViewButton.TintColor = UIColor.Gray;
			};
			Add (showActiveOrdersViewButton);

			offsetYbutton += heightButton - 0.5f;
			showEditProfileViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			showEditProfileViewButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			showEditProfileViewButton.TintColor = UIColor.Gray;
			showEditProfileViewButton.SetTitle ("      Editar Perfil", UIControlState.Normal);
			showEditProfileViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showEditProfileViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showEditProfileViewButton.Layer.BorderWidth = 0.5f;
			showEditProfileViewButton.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.7f);
			showEditProfileViewButton.TouchUpInside += delegate {
				viewModel.DoOpenEditProfile.Execute(null);
				typeViewOpened = TypeViewOpened.EDIT_PROFILE;
				showRegisterOrderViewButton.TintColor = UIColor.Gray;
				showActiveOrdersViewButton.TintColor = UIColor.Gray;
				showEditProfileViewButton.TintColor = Constants.TINT_COLOR;
			};
			Add (showEditProfileViewButton);
        }
    }
}

