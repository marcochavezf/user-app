using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using Kangou.Core;
using Kangou.Helpers;
using System.Threading.Tasks;
using SlidingPanels.Lib;

namespace Kangou.Touch.Views
{
	[Register("CreditCardView")]
	public class CreditCardView : MvxViewController
	{
		private BindableProgress _bindableProgress;

		public override void ViewDidLoad()
		{
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH / 8; 
			var MARGIN_WIDTH_TEXTVIEW = MARGIN_WIDTH_SUBVIEWS * 1.5f;
			var HEIGHT_TEXTFIELD = HEIGHT * 0.075f;
			var HEIGHT_TEXTVIEW = HEIGHT * 0.3f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

			var pYoffset = HEIGHT * 0.22f;

			//CreditCard Number
			var creditCardNumberTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			creditCardNumberTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			creditCardNumberTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			creditCardNumberTextField.Layer.BorderWidth = 0.5f;
			creditCardNumberTextField.Layer.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR.CGColor;
			creditCardNumberTextField.TextAlignment = UITextAlignment.Center;
			creditCardNumberTextField.Placeholder = "1234 5678 9012 3456";
			creditCardNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
			creditCardNumberTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			creditCardNumberTextField.EditingChanged += (object sender, System.EventArgs e) => {
				var creditCardNumber = creditCardNumberTextField.Text.Trim();
				if(creditCardNumber.Length == 20)
					creditCardNumber = creditCardNumber.Remove(19);
				creditCardNumberTextField.Text = StringFormater.FormatCreditCardNumber(creditCardNumber);
			};
			Add (creditCardNumberTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//Expiration Date
			var expirationDateTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			expirationDateTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			expirationDateTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			expirationDateTextField.Layer.BorderWidth = 0.5f;
			expirationDateTextField.Layer.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR.CGColor;
			expirationDateTextField.TextAlignment = UITextAlignment.Center;
			expirationDateTextField.Placeholder = "MM/YY";
			expirationDateTextField.KeyboardType = UIKeyboardType.NumberPad;
			expirationDateTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			expirationDateTextField.EditingChanged += (object sender, System.EventArgs e) => {
				var expirationDateString = expirationDateTextField.Text.Trim();
				if(expirationDateString.Length == 6)
					expirationDateString = expirationDateString.Remove(5);
				expirationDateTextField.Text = StringFormater.FormatExpirationDate(expirationDateString);
			};
			Add (expirationDateTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//CVV
			var cvvTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			cvvTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			cvvTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			cvvTextField.Layer.BorderWidth = 0.5f;
			cvvTextField.Layer.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR.CGColor;
			cvvTextField.TextAlignment = UITextAlignment.Center;
			cvvTextField.Placeholder = "CVV";
			cvvTextField.KeyboardType = UIKeyboardType.NumberPad;
			cvvTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			cvvTextField.EditingChanged += (object sender, System.EventArgs e) => {
				var cvvString = cvvTextField.Text.Trim();
				if(cvvString.Length == 5)
					cvvTextField.Text = cvvString.Remove(4);
			};
			Add (cvvTextField);
			pYoffset += HEIGHT_TEXTFIELD * 1.85f;

			//Info Pick Up Text View
			var aboutChargeTextView = new UILabel(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTVIEW));
			aboutChargeTextView.Text = "Se realizará el cargo del envío una vez que se haya confirmado la orden.\n\nEn caso de compra, se realizará un segundo cargo: la compra más la comisión del 5% de la misma.\n\nEl Kangou te avisará del monto total antes de hacer la compra.";
			aboutChargeTextView.Lines = 0;
			aboutChargeTextView.Font = UIFont.FromName(LABEL_FONT, 12f);
			aboutChargeTextView.TextColor = UIColor.Black;
			aboutChargeTextView.TextAlignment = UITextAlignment.Center;
			aboutChargeTextView.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.5f);
			aboutChargeTextView.Layer.BorderColor = UIColor.DarkGray.CGColor;
			aboutChargeTextView.Layer.BorderWidth = 0.5f;
			Add(aboutChargeTextView);

			//Toolbar with Done Button for FullName
			var toolbarFullName = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarFullName.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					expirationDateTextField.BecomeFirstResponder();
				})
			};
			creditCardNumberTextField.InputAccessoryView = toolbarFullName;

			//Toolbar with Done Button for PhoneNumber
			var toolbarPhoneNumber = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarPhoneNumber.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					cvvTextField.BecomeFirstResponder();
				})
			};
			expirationDateTextField.InputAccessoryView = toolbarPhoneNumber;

			//Toolbar with Done Button for Email
			var toolbarEmail = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarEmail.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					cvvTextField.ResignFirstResponder();
				})
			};
			cvvTextField.InputAccessoryView = toolbarEmail;

			_bindableProgress = new BindableProgress(View);

			//Binding
			var set = this.CreateBindingSet<CreditCardView, CreditCardViewModel>();
			set.Bind(creditCardNumberTextField).To(vm => vm.CreditCardNumber);
			set.Bind(expirationDateTextField).To(vm => vm.ExpirationDate);
			set.Bind(cvvTextField).To(vm => vm.CVV);
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
			set.Apply();

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender,args) => {

					var creditCardString = creditCardNumberTextField.Text.Trim ();
					var expirationDateString = expirationDateTextField.Text.Trim ();
					var cvvString = cvvTextField.Text.Trim ();

					//Check if data is well formatted, otherwise publish data to ViewModel
					if (creditCardString.Equals("") || !StringValidator.IsValidCreditCard (creditCardString)){
						createAlert(creditCardNumberTextField,"Se requiere un número de tarjeta válido");
					}else

					if (expirationDateString.Equals ("") || expirationDateString.Length < 4){
						createAlert(expirationDateTextField,"Fecha de expiración inválida");
					}else

					if (cvvString.Equals("") || cvvString.Length < 3){
						createAlert(cvvTextField,"CVV inválido");
						
					}else{
						creditCardNumberTextField.ResignFirstResponder();
						expirationDateTextField.ResignFirstResponder();
						cvvTextField.ResignFirstResponder();
						processData();
					}

				})
				, true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			SlidingGestureRecogniser.EnableGesture = false;
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			SlidingGestureRecogniser.EnableGesture = true;
		}


		private void processData(){

			//Check if Internet is available
			var networkStatus = Reachability.RemoteHostStatus ();
			System.Diagnostics.Debug.WriteLine (networkStatus);
			if(networkStatus == NetworkStatus.NotReachable) {
				new UIAlertView ("No hay conexión a Iternet", ""
					, null, "Ok", null).Show ();
			}
			else
			{

				//Publish Data (Send it to server)
				var viewModel = (CreditCardViewModel)ViewModel;
				viewModel.IsBusy = true;

				Task.Run (()=>{
					var isSuccesful = viewModel.PublishData ();

					System.Diagnostics.Debug.WriteLine("isSuccesful: {0}",isSuccesful);
					InvokeOnMainThread (delegate {  

						viewModel.IsBusy = false;

						if(isSuccesful)
							NavigationController.PopViewControllerAnimated(true);
						else
							new UIAlertView ("Error al registrar datos", "Favor de verificar que sus datos sean correctos o que tenga conexión a Internet", null, "Ok", null).Show ();
					});

				});
			}

		}

		private void createAlert(UIView subview, string message){
			var alert = new UIAlertView(message, ""
				, null, "Ok", null);
			alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
				if(e.ButtonIndex == 0)
					subview.BecomeFirstResponder();
			};
			alert.Show();
		}



	}
}