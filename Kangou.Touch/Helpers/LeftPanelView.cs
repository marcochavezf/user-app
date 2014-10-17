using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using System;

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

        public override void ViewDidLoad ()
        {
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;

			View = new UIView (){ BackgroundColor = UIColor.White };
			View.Frame = new RectangleF (0, 0, 250, HEIGHT);

			base.ViewDidLoad();


			var widthButton = WIDTH;
			var heightButton = 70f;
			var posXbutton = -1f;
			var offsetYbutton = 20f;
			var showRegisterOrderViewButton = new UIButton (UIButtonType.RoundedRect);
			showRegisterOrderViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			//showRegisterOrderViewButton.TintColor = UIColor.Orange;
			showRegisterOrderViewButton.SetTitle ("        Crear Orden", UIControlState.Normal);
			showRegisterOrderViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showRegisterOrderViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showRegisterOrderViewButton.Layer.BorderWidth = 0.0f;
			Add (showRegisterOrderViewButton);

			offsetYbutton += heightButton - 0.5f;
			var showActiveOrdersViewButton = new UIButton (UIButtonType.RoundedRect);
			showActiveOrdersViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			//showActiveOrdersViewButton.TintColor = UIColor.DarkGray;
			showActiveOrdersViewButton.SetTitle ("        Órdenes Activas", UIControlState.Normal);
			showActiveOrdersViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showActiveOrdersViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showActiveOrdersViewButton.Layer.BorderWidth = 0.5f;
			Add (showActiveOrdersViewButton);

			offsetYbutton += heightButton - 0.5f;
			var showEditProfileViewButton = new UIButton (UIButtonType.RoundedRect);
			showEditProfileViewButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			//showEditProfileViewButton.TintColor = UIColor.DarkGray;
			showEditProfileViewButton.SetTitle ("        Editar Perfil", UIControlState.Normal);
			showEditProfileViewButton.Frame = new RectangleF (posXbutton, offsetYbutton, widthButton, heightButton);
			showEditProfileViewButton.Layer.BorderColor = UIColor.Gray.CGColor;
			showEditProfileViewButton.Layer.BorderWidth = 0.5f;
			Add (showEditProfileViewButton);

			this.AddBindings(
				new Dictionary<object, string>()
				{
					{showRegisterOrderViewButton, "TouchUpInside DoOpenRegisterOrder"},
					{showActiveOrdersViewButton, "TouchUpInside DoOpenActiveOrdersList"},
					{showEditProfileViewButton, "TouchUpInside DoOpenEditProfile"}
				});
				
        }
    }
}

