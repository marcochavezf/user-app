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
		private UIButton _priceDistanceButton;

		public override void ViewDidLoad()
		{
			NavigationItem.Title = "Crear orden";
			base.ViewDidLoad();
			var locMngr = LocationManager.Instance;
			locMngr.StartLocationUpdates ();

			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_SECONDARY;

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
			var itemsLabel = new UIButton (UIButtonType.RoundedRect);
			itemsLabel.SetTitle ("1. ¿Qué comprar o traer?", UIControlState.Normal);
			itemsLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsLabel.TintColor = UIColor.Gray;
			itemsLabel.Frame = new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20);
			Add (itemsLabel);
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
			var pickUpLabel = new UIButton (UIButtonType.RoundedRect);
			pickUpLabel.SetTitle ("2. ¿Dónde recoger?", UIControlState.Normal);
			pickUpLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			pickUpLabel.TintColor = UIColor.Gray;
			pickUpLabel.Frame = new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20);
			Add (pickUpLabel);
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
			var dropOffLabel = new UIButton (UIButtonType.RoundedRect);
			dropOffLabel.SetTitle ("3. ¿Dónde entregar?", UIControlState.Normal);
			dropOffLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			dropOffLabel.TintColor = UIColor.Gray;
			dropOffLabel.Frame = new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20);
			Add (dropOffLabel);
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
			var paymentMethodLabel = new UIButton (UIButtonType.RoundedRect);
			paymentMethodLabel.SetTitle ("4. Medio de pago", UIControlState.Normal);
			paymentMethodLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			paymentMethodLabel.TintColor = UIColor.Gray;
			paymentMethodLabel.Frame = new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20);
			Add (paymentMethodLabel);
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

			pYoffset += PADDING_SECTION * 1.25f;

			//Confirm Order Alert
			var confirmOrderAlert = new UIAlertView ("Confirmar Orden","", null, "Cancelar", "Confirmar");
			confirmOrderAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
				switch (eventArgsAlert.ButtonIndex) {

				//Confirm
				case 1:
					_viewModel.PushDeviceToken =  NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
					ConfirmOrderClicked();
					break;

				}
			};

			EventHandler actionPideUnKangou = (object sender, System.EventArgs e) => {
				if (_viewModel.IsDeliveryDataSet ()) {
					if (_viewModel.IsPaymentInfoSet ()){

						var km = RecomputePriceAndGetDistance();
						if(km > 25){
							new UIAlertView ("Distancias muy retiradas","La distancia entre el punto de recogida y el punto de entrega es muy lejos", null, "Ok").Show();
							return;
						}

						if(_viewModel.IsAPurchase)
							confirmOrderAlert.Message = String.Format(StringMessages.CONFIRM_MESSAGE_PURCHASE_IOS, _viewModel.PriceInPesos, km);
						else
							confirmOrderAlert.Message = String.Format(StringMessages.CONFIRM_MESSAGE_IOS, _viewModel.PriceInPesos, km);
						confirmOrderAlert.Show ();
					}
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
			pideUnKangouButton.TouchUpInside += actionPideUnKangou;
			Add (pideUnKangouButton);
			pYoffset += HEIGHT_BUTTON;

			_priceDistanceButton = new UIButton (UIButtonType.RoundedRect);
			_priceDistanceButton.SetTitle ("", UIControlState.Normal);
			_priceDistanceButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);
			_priceDistanceButton.TintColor = UIColor.Orange;
			_priceDistanceButton.Frame = new RectangleF(MARGIN_BUTTONS, pYoffset, WIDTH-MARGIN_BUTTONS*2, HEIGHT_BUTTON* 2);
			_priceDistanceButton.LineBreakMode = UILineBreakMode.WordWrap;
			_priceDistanceButton.TitleLabel.TextAlignment = UITextAlignment.Center;
			_priceDistanceButton.Layer.BorderColor = UIColor.Gray.CGColor;
			_priceDistanceButton.Layer.BorderWidth = BORDER_WIDTH;
			_priceDistanceButton.TouchUpInside += actionPideUnKangou;
			Add (_priceDistanceButton);

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

			set.Bind(itemsLabel).To (vm => vm.AddItemsCommand);
			set.Bind(pickUpLabel).To (vm => vm.AddPickUpAddressCommand);
			set.Bind(dropOffLabel).To(vm => vm.AddDropOffAddressCommand);
			set.Bind(paymentMethodLabel).To(vm => vm.AddCreditCardCommand);
			set.Apply();

			_viewModel.EnablePickUpButton = delegate {
				InvokeOnMainThread (delegate {  
					itemsButton.TintColor = UIColor.DarkGray;
					itemsButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);

					pickUpButton.Enabled = true;
					pickUpButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					pickUpLabel.Enabled = true;
				});
			};

			_viewModel.EnableDropOffButton = delegate {
				_viewModel.EnablePickUpButton();
				InvokeOnMainThread (delegate {
					pickUpButton.TintColor = UIColor.DarkGray;
					pickUpButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);

					dropOffButton.Enabled = true;
					dropOffButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					dropOffLabel.Enabled = true;
				});
			};

			_viewModel.EnablePaymentMethodButton = delegate {
				_viewModel.EnableDropOffButton();
				InvokeOnMainThread (delegate {  
					dropOffButton.TintColor = UIColor.DarkGray;
					dropOffButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);

					paymentMethodButton.Enabled = true;
					paymentMethodButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);
					paymentMethodLabel.Enabled = true;
				});
			};

			_viewModel.EnablePUKButton = delegate {
				_viewModel.EnablePaymentMethodButton ();
				InvokeOnMainThread (delegate {  
					paymentMethodButton.TintColor = UIColor.DarkGray;
					paymentMethodButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_A);

					pideUnKangouButton.Enabled = true;
					_priceDistanceButton.Enabled = true;
				});
				RecomputePriceAndGetDistance();
			};

			_viewModel.DisableButtons = delegate {
				InvokeOnMainThread (delegate {  
					pickUpButton.Enabled = false;
					dropOffButton.Enabled = false;
					paymentMethodButton.Enabled = false;
					pideUnKangouButton.Enabled = false;
					_priceDistanceButton.Enabled = false;

					itemsButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					pickUpButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					dropOffButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					paymentMethodButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					pideUnKangouButton.TintColor = Constants.TINT_COLOR_PRIMARY;
					_priceDistanceButton.TintColor = Constants.TINT_COLOR_PRIMARY;

					itemsButton.Font = UIFont.FromName(Constants.BUTTON_FONT, Constants.BUTTON_FONT_SIZE_B);

					pickUpLabel.Enabled = false;
					dropOffLabel.Enabled = false;
					paymentMethodLabel.Enabled = false;
				});
			};
			_viewModel.DisableButtons ();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Cancelar", UIBarButtonItemStyle.Plain, null); 
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_viewModel.PublishMessageViewOpened ();
			RecomputePriceAndGetDistance ();
		}

		private double RecomputePriceAndGetDistance(){
			if (_viewModel.PickUpData  == null
			||  _viewModel.DropOffData == null)
				return 0;

			var origin = new CLLocation(_viewModel.PickUpData.Lat, _viewModel.PickUpData.Lng); 
			var destiny = new CLLocation(_viewModel.DropOffData.Lat, _viewModel.DropOffData.Lng);
			var meters = destiny.DistanceFrom(origin);
			var km = Math.Round(meters/1000.0f, 1);
			var priceInPesos = 50;
			string titlePriceDistanceButton = "";

			if(km > 7 && km <= 10)
				priceInPesos = 80;
			else
				if(km > 10 && km <= 15)
					priceInPesos = 120;
				else
					if(km > 15 && km <= 20)
						priceInPesos = 170;
					else
						if(km > 20 && km <= 25)
							priceInPesos = 230;
							
			titlePriceDistanceButton = String.Format ("Distancia: {0} km\nCosto del envío: ${1}.00", km, priceInPesos);
			if(km > 25)
				titlePriceDistanceButton = "Distancia mayor a 25 km";

			_viewModel.PriceInPesos = priceInPesos;
			_viewModel.DistancePickUpToDropOff = String.Format("{0} Km",km.ToString());
			InvokeOnMainThread (delegate {
				_priceDistanceButton.SetTitle (titlePriceDistanceButton, UIControlState.Normal);
			});
			return km;
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
						() => {
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