using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using System.Collections.Generic;
using System;
using Kangou.Touch.Helpers;
using Kangou.Core.Helpers;
using Kangou.Helpers;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreLocation;
using System.Threading.Tasks;
using System.Diagnostics;
using SlidingPanels.Lib;

namespace Kangou.Touch.Views
{
	[Register("RegisterOrderView")]
	public class RegisterOrderView : RootMvxViewController
	{
		private const int CREDIT_CARD_ID = 0;
		private const int CASH_ID = 1;
		private RegisterOrderViewModel _viewModel;
		private CLLocationManager locMgr;

		public override void ViewDidLoad()
		{
			NavigationItem.Title = "Crear orden";
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_SECONDARY;

			locMgr = new CLLocationManager();
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 8)) {
				locMgr.RequestWhenInUseAuthorization ();
			}

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH / 8;
			var MARGIN_BUTTONS = WIDTH * 0.05f;
			var PADDING_LABEL_BUTTON = HEIGHT * 0.05f;
			var PADDING_SECTION = HEIGHT * 0.1f;
			var HEIGHT_BUTTON = HEIGHT * 0.035f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";
			var BORDER_WIDTH = 0.0f;
			_viewModel = (RegisterOrderViewModel)ViewModel;

			var pYoffset = HEIGHT * 0.2f;

			//Items Label
			var itemsLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20));
			itemsLabel.Text = "1. ¿Qué comprar o traer?";
			itemsLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsLabel.TextColor = UIColor.Gray;
			itemsLabel.TextAlignment = UITextAlignment.Center;
			Add(itemsLabel);
			pYoffset += PADDING_LABEL_BUTTON;

			//Items Button
			var itemsButton = new UIButton (UIButtonType.RoundedRect);
			itemsButton.SetTitle ("Agregar productos", UIControlState.Normal);
			itemsButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
			itemsButton.TintColor = UIColor.Orange;
			itemsButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON);
			itemsButton.Layer.BorderColor = UIColor.Gray.CGColor;
			itemsButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (itemsButton);
			pYoffset += PADDING_SECTION;

			//Items Label
			var pickUpLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20));
			pickUpLabel.Text = "2. ¿Dónde recoger?";
			pickUpLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			pickUpLabel.TextColor = UIColor.Gray;
			pickUpLabel.TextAlignment = UITextAlignment.Center;
			Add(pickUpLabel);
			pYoffset += PADDING_LABEL_BUTTON;

			//Items Button
			var pickUpButton = new UIButton (UIButtonType.RoundedRect);
			pickUpButton.SetTitle ("Agregar dirección", UIControlState.Normal);
			pickUpButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			pickUpButton.TintColor = UIColor.Orange;
			pickUpButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON);
			pickUpButton.Layer.BorderColor = UIColor.Gray.CGColor;
			pickUpButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (pickUpButton);
			pYoffset += PADDING_SECTION;

			//Drop Off Label
			var dropOffLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20));
			dropOffLabel.Text = "3. ¿Dónde entregar?";
			dropOffLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			dropOffLabel.TextColor = UIColor.Gray;
			dropOffLabel.TextAlignment = UITextAlignment.Center;
			Add(dropOffLabel);
			pYoffset += PADDING_LABEL_BUTTON;

			//Drop Off Button
			var dropOffButton = new UIButton (UIButtonType.RoundedRect);
			dropOffButton.SetTitle ("Agregar dirección", UIControlState.Normal);
			dropOffButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			dropOffButton.TintColor = UIColor.Orange;
			dropOffButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON);
			dropOffButton.Layer.BorderColor = UIColor.Gray.CGColor;
			dropOffButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (dropOffButton);

			pYoffset += PADDING_SECTION;

			//Payment Method Label
			var paymentMethodLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20));
			paymentMethodLabel.Text = "4. Medio de pago";
			paymentMethodLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			paymentMethodLabel.TextColor = UIColor.Gray;
			paymentMethodLabel.TextAlignment = UITextAlignment.Center;
			Add(paymentMethodLabel);
			pYoffset += PADDING_LABEL_BUTTON;

			//Payment Method Button
			var paymentMethodButton = new UIButton (UIButtonType.RoundedRect);
			paymentMethodButton.SetTitle ("Seleccionar forma de pago", UIControlState.Normal);
			paymentMethodButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			paymentMethodButton.TintColor = UIColor.Orange;
			paymentMethodButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON);
			paymentMethodButton.Layer.BorderColor = UIColor.Gray.CGColor;
			paymentMethodButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (paymentMethodButton);

			pYoffset += PADDING_SECTION * 1.5f;

			//Confirm Order Alert
			var confirmOrderAlert = new UIAlertView ("Confirmar Orden",StringMessages.CONFIRM_MESSAGE_IOS, null, "Cancelar", "Confirmar");
			confirmOrderAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
				switch (eventArgsAlert.ButtonIndex) {

				//Confirm
				case 1:
					ConfirmOrderClicked();
					break;

				}
			};

			//Pedir un Kangou! Button
			var pideUnKangouButton = new UIButton (UIButtonType.RoundedRect);
			pideUnKangouButton.SetTitle ("Pedir un Kangou", UIControlState.Normal);
			pideUnKangouButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_C);
			pideUnKangouButton.TintColor = UIColor.Orange;
			pideUnKangouButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON);
			pideUnKangouButton.Layer.BorderColor = UIColor.Gray.CGColor;
			pideUnKangouButton.Layer.BorderWidth = BORDER_WIDTH;
			pideUnKangouButton.TouchUpInside += (object sender, System.EventArgs e) => {
				if (_viewModel.IsDeliveryDataSet ()) {
					if (_viewModel.IsPaymentInfoSet ())
						confirmOrderAlert.Show ();
				}
			};
			Add (pideUnKangouButton);

			//Data Binding
			var set = this.CreateBindingSet<RegisterOrderView, RegisterOrderViewModel>();
			set.Bind(itemsButton).For("Title").To(vm => vm.Items);
			set.Bind(pickUpButton).For("Title").To(vm => vm.PickUpAddress);
			set.Bind(dropOffButton).For("Title").To(vm => vm.DropOffAddress);
			set.Bind(paymentMethodButton).For("Title").To(vm => vm.InfoPaymentMethod);
			set.Bind(itemsButton).To (vm => vm.AddItemsCommand);
			set.Bind(pickUpButton).To (vm => vm.AddPickUpAddressCommand);
			set.Bind(dropOffButton).To(vm => vm.AddDropOffAddressCommand);
			set.Bind(paymentMethodButton).To(vm => vm.AddCreditCardCommand);
			set.Apply();

			_viewModel.EnablePickUpButton = delegate {
				InvokeOnMainThread (delegate {  
					itemsButton.TintColor = UIColor.DarkGray;
					itemsButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
					pickUpButton.Enabled = true;
					pickUpButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					pickUpLabel.TextColor = UIColor.Gray;
				});
			};

			_viewModel.EnableDropOffButton = delegate {
				_viewModel.EnablePickUpButton();
				InvokeOnMainThread (delegate {
					pickUpButton.TintColor = UIColor.DarkGray;
					pickUpButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
					dropOffButton.Enabled = true;
					dropOffButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					dropOffLabel.TextColor = UIColor.Gray;
				});
			};

			_viewModel.EnablePaymentMethodButton = delegate {
				_viewModel.EnableDropOffButton();
				InvokeOnMainThread (delegate {  
					dropOffButton.TintColor = UIColor.DarkGray;
					dropOffButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
					paymentMethodButton.Enabled = true;
					paymentMethodButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					paymentMethodLabel.TextColor = UIColor.Gray;
				});
			};

			_viewModel.EnablePUKButton = delegate {
				_viewModel.EnablePaymentMethodButton ();
				InvokeOnMainThread (delegate {  
					paymentMethodButton.TintColor = UIColor.DarkGray;
					paymentMethodButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
					pideUnKangouButton.Enabled = true;
				});
			};

			_viewModel.DisableButtons = delegate {
				InvokeOnMainThread (delegate {  
					pickUpButton.Enabled = false;
					dropOffButton.Enabled = false;
					paymentMethodButton.Enabled = false;
					pideUnKangouButton.Enabled = false;

					itemsButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					pickUpButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					dropOffButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					paymentMethodButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					pideUnKangouButton.TintColor = Constants.TINT_COLOR_PRIMARY;

					itemsButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);

					pickUpLabel.TextColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
					dropOffLabel.TextColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
					paymentMethodLabel.TextColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
				});
			};
			_viewModel.DisableButtons ();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Cancelar", UIBarButtonItemStyle.Plain, null); 
		}


		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait ;

		}

		public override bool ShouldAutorotate ()
		{
			return false;
		}

		private void ConfirmOrderClicked()
		{
			//Check if Internet is available
			if(Reachability.RemoteHostStatus () == NetworkStatus.NotReachable) {
				new UIAlertView ("No hay conexión a Iternet", ""
					, null, "Ok", null).Show ();
			}else{

				//Set Progress Dialog
				// Determine the correct size to start the overlay (depending on device orientation)
				var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
				if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
					bounds.Size = new SizeF(bounds.Size.Height, bounds.Size.Width);
				}
				// show the loading overlay on the UI thread using the correct orientation sizing
				var loadingOverlay = new LoadingOverlay (bounds);
				this.View.Add ( loadingOverlay );

				//Create Error Oder Response Dialog
				var errorOrderResponseAlert = new UIAlertView (StringMessages.ERROR_ORDER_RESPONSE_TITLE,StringMessages.ERROR_ORDER_RESPONSE_MESSAGE, null, "Ok", null);

				try
				{
					_viewModel.ConfirmOrder (
						(sucessResponse) => {
							InvokeOnMainThread (delegate {  
								loadingOverlay.Hide ();
							});
						},
						(errorResponse) => {
							InvokeOnMainThread (delegate {  
								loadingOverlay.Hide (()=>{
									errorOrderResponseAlert.Show ();
								});
							});
						});

				}catch (Exception e){
					errorOrderResponseAlert.Show ();
					loadingOverlay.Hide (()=>{
						errorOrderResponseAlert.Show ();
					});
				}

			}

		}
			
	}
}