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

namespace Kangou.Touch.Views
{
	[Register("RegisterOrderView")]
	public class RegisterOrderView : MvxViewController
	{
		private const int CREDIT_CARD_ID = 0;
		private const int CASH_ID = 1;
		private RegisterOrderViewModel _viewModel;

		public override void ViewDidLoad()
		{
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH / 8; 
			var PADDING_LABEL_BUTTON = 25;
			var PADDING_SECTION = 80;
			var HEIGHT_BUTTON = 50;
			var LABEL_FONT_SIZE = 15f;
			var BUTTON_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";
			var BUTTON_FONT = "Arial-BoldMT";
			var BORDER_WIDTH = 0.5f;
			_viewModel = (RegisterOrderViewModel)ViewModel;

			var pY = 40f;

			//Items Label
			var itemsLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			itemsLabel.Text = "1. ¿Qué comprar o traer?";
			itemsLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsLabel.TextColor = UIColor.Gray;
			itemsLabel.TextAlignment = UITextAlignment.Center;
			Add(itemsLabel);
			pY += PADDING_LABEL_BUTTON;

			//Items Button
			var itemsButton = new UIButton (UIButtonType.RoundedRect);
			itemsButton.SetTitle ("Agregar productos", UIControlState.Normal);
			itemsButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			itemsButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			itemsButton.Layer.BorderColor = UIColor.Gray.CGColor;
			itemsButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (itemsButton);

			pY += PADDING_SECTION;

			//Drop Off Label
			var dropOffLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			dropOffLabel.Text = "2. ¿Dónde entregar?";
			dropOffLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			dropOffLabel.TextColor = UIColor.Gray;
			dropOffLabel.TextAlignment = UITextAlignment.Center;
			Add(dropOffLabel);
			pY += PADDING_LABEL_BUTTON;

			//Drop Off Button
			var dropOffButton = new UIButton (UIButtonType.RoundedRect);
			dropOffButton.SetTitle ("Agregar dirección", UIControlState.Normal);
			dropOffButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			dropOffButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			dropOffButton.Layer.BorderColor = UIColor.Gray.CGColor;
			dropOffButton.Layer.BorderWidth = BORDER_WIDTH;
			Add (dropOffButton);

			pY += PADDING_SECTION;

			//Payment Method Label
			var paymentMethodLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			paymentMethodLabel.Text = "3. Información de pago";
			paymentMethodLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			paymentMethodLabel.TextColor = UIColor.Gray;
			paymentMethodLabel.TextAlignment = UITextAlignment.Center;
			Add(paymentMethodLabel);
			pY += PADDING_LABEL_BUTTON;

			//Payment Method Action Sheet
			var paymentMethodactionSheet = new UIActionSheet ("Forma de pago");
			paymentMethodactionSheet.Center = new PointF(100,100);
			paymentMethodactionSheet.AddButton ("Tarjeta de Crédito");
			paymentMethodactionSheet.AddButton ("Efectivo");
			paymentMethodactionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
				switch (b.ButtonIndex) {

				case CREDIT_CARD_ID:
					_viewModel.AddCreditCardCommand.Execute (null);
					break;

				case CASH_ID:
					_viewModel.SetCashPaymentMethod();
					break;

				default:
					_viewModel.InfoPaymentMethod = "Seleccionar forma de pago";
					break;

				}
			};

			//Payment Method Button
			var paymentMethodButton = new UIButton (UIButtonType.RoundedRect);
			paymentMethodButton.SetTitle ("Seleccionar forma de pago", UIControlState.Normal);
			paymentMethodButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			paymentMethodButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			paymentMethodButton.Layer.BorderColor = UIColor.Gray.CGColor;
			paymentMethodButton.Layer.BorderWidth = BORDER_WIDTH;
			paymentMethodButton.TouchUpInside += (object sender, System.EventArgs e) => {
				paymentMethodactionSheet.ShowInView (View);
			};
			Add (paymentMethodButton);

			pY += PADDING_SECTION * 1.75f;

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
			pideUnKangouButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			pideUnKangouButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			pideUnKangouButton.Layer.BorderColor = UIColor.Gray.CGColor;
			pideUnKangouButton.Layer.BorderWidth = BORDER_WIDTH;
			pideUnKangouButton.TouchUpInside += (object sender, System.EventArgs e) => {
				if (_viewModel.IsDeliveryDataSet ()) {
					if (_viewModel.IsPaymentInfoSet ())
						confirmOrderAlert.Show ();
					else
						paymentMethodactionSheet.ShowInView (View);
				}
			};
			Add (pideUnKangouButton);

			//Data Binding
			var set = this.CreateBindingSet<RegisterOrderView, RegisterOrderViewModel>();
			set.Bind(itemsButton).For("Title").To(vm => vm.Items);
			set.Bind(dropOffButton).For("Title").To(vm => vm.DropOffAddress);
			set.Bind(paymentMethodButton).For("Title").To(vm => vm.InfoPaymentMethod);
			set.Bind(itemsButton).To (vm => vm.AddItemsCommand);
			set.Bind(dropOffButton).To(vm => vm.AddDropOffAddressCommand);
			set.Apply();

			_viewModel.EnableDropOffButton = ()=>{
				dropOffButton.Enabled = true;
				dropOffLabel.TextColor = UIColor.Gray;
			};

			_viewModel.EnablePaymentMethodButton = ()=>{
				_viewModel.EnableDropOffButton();
				paymentMethodButton.Enabled = true;
				paymentMethodLabel.TextColor = UIColor.Gray;
			};

			_viewModel.EnablePUKButton = ()=>{
				_viewModel.EnablePaymentMethodButton ();
				pideUnKangouButton.Enabled = true;
			};

			_viewModel.DisableButtons = () => {
				dropOffButton.Enabled = false;
				paymentMethodButton.Enabled = false;
				pideUnKangouButton.Enabled = false;

				dropOffLabel.TextColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
				paymentMethodLabel.TextColor = UIColor.FromWhiteAlpha(0.5f, 0.5f);
			};

			_viewModel.DisableButtons ();
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