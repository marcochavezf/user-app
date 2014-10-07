using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using System;
using Kangou.Core;

namespace Kangou.Touch.Views
{
	[Register("DropOffView")]
	public class DropOffView : MvxViewController
	{
		public override void ViewDidLoad()
		{
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH / 8 * 0.5f; 
			var MARGIN_HEIGHT_SUBVIEWS = MARGIN_WIDTH_SUBVIEWS;
			var HEIGHT_TEXTFIELD = 40f;
			var HEIGHT_TEXTVIEW = 67.5f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			var viewModel = (DropOffViewModel)ViewModel;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			var pYoffset = MARGIN_HEIGHT_SUBVIEWS;

			//FullName
			var fullNameTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			fullNameTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			fullNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			fullNameTextField.Layer.BorderWidth = 0.5f;
			fullNameTextField.TextAlignment = UITextAlignment.Center;
			fullNameTextField.Placeholder = "Nombre";
			Add (fullNameTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//PhoneNumber
			var phoneNumberTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			phoneNumberTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			phoneNumberTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			phoneNumberTextField.Layer.BorderWidth = 0.5f;
			phoneNumberTextField.TextAlignment = UITextAlignment.Center;
			phoneNumberTextField.Placeholder = "Teléfono";
			phoneNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
			Add (phoneNumberTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//Email
			var emailTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			emailTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			emailTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			emailTextField.Layer.BorderWidth = 0.5f;
			emailTextField.TextAlignment = UITextAlignment.Center;
			emailTextField.Placeholder = "Email";
			emailTextField.KeyboardType = UIKeyboardType.EmailAddress;
			emailTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			Add (emailTextField);
			pYoffset += HEIGHT_TEXTFIELD-1;

			//DropOff Address Text Field
			var addressTextField = new UITextView(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTVIEW));
			addressTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			addressTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			addressTextField.Layer.BorderWidth = 0.5f;
			addressTextField.TextAlignment = UITextAlignment.Center;
			Add (addressTextField);
			pYoffset += HEIGHT_TEXTVIEW + MARGIN_HEIGHT_SUBVIEWS;


			//Toolbar with Done Button for FullName
			var toolbarFullName = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarFullName.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					phoneNumberTextField.BecomeFirstResponder();
				})
			};
			fullNameTextField.InputAccessoryView = toolbarFullName;

			//Toolbar with Done Button for PhoneNumber
			var toolbarPhoneNumber = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarPhoneNumber.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					emailTextField.BecomeFirstResponder();
				})
			};
			phoneNumberTextField.InputAccessoryView = toolbarPhoneNumber;

			//Toolbar with Done Button for Email
			var toolbarEmail = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarEmail.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					emailTextField.ResignFirstResponder();
				})
			};
			emailTextField.InputAccessoryView = toolbarEmail;

			//Toolbar with Done Button for Address
			var toolbarAddress = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarAddress.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					addressTextField.ResignFirstResponder();
				})
			};
			addressTextField.InputAccessoryView = toolbarAddress;

			//Binding
			var set = this.CreateBindingSet<DropOffView, DropOffViewModel>();
			set.Bind(fullNameTextField).To(vm => vm.FullName);
			set.Bind(phoneNumberTextField).To(vm => vm.PhoneNumber);
			set.Bind(emailTextField).To(vm => vm.Email);
			set.Bind(addressTextField).To(vm => vm.Address);
			set.Apply();

			//Adding map
			var mapView = new MKMapView ();
			mapView.SetRegion (MKCoordinateRegion.FromDistance (new CLLocationCoordinate2D (19.430566, -99.200946), 1000, 1000), true);
			mapView.Frame = new RectangleF (0, pYoffset, CONTAINER_SIZE.Width, CONTAINER_SIZE.Height - pYoffset);
			mapView.ShowsBuildings = true;
			mapView.PitchEnabled = true;
			mapView.ShowsUserLocation = true;
			var tapGesture = new UITapGestureRecognizer ((g) => {
				addressTextField.ResignFirstResponder();
			});
			mapView.AddGestureRecognizer (tapGesture);
			Add (mapView);
			viewModel.Address = "Cargando...";
			pYoffset += mapView.Frame.Height * 0.5f;

			//Adding Pin
			var kangouMarker = new UIImageView (UIImage.FromBundle("ic_marker.png"));
			var widthMarker = 50f;
			var heightMarker = 60f;
			kangouMarker.Frame = new RectangleF (CONTAINER_SIZE.Width*0.5f - widthMarker*0.5f, pYoffset - heightMarker*1.525f, widthMarker, heightMarker);
			Add (kangouMarker);

			//Getting variables to set address
			var clg = new CLGeocoder();
			var address = "";
			var isoCountryCode = "";
			var cllocation = new CLLocation(mapView.Region.Center.Latitude, mapView.Region.Center.Longitude);

			//Getting address first time
			clg.ReverseGeocodeLocation(cllocation, (CLPlacemark[] placemarks, NSError error)=>{
				if (placemarks == null)
					return;
				for(var i = 0; i < placemarks.Length; i++)
				{
					var loc = placemarks[i];
					address = String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
						loc.Name, loc.SubLocality, loc.Locality,
						loc.AdministrativeArea, loc.Country, loc.PostalCode); 
					isoCountryCode = loc.IsoCountryCode;
					viewModel.Address = address;
				}
			});

			//Getting address when user move region map
			mapView.RegionWillChange += (object sender, MKMapViewChangeEventArgs e) => {
				viewModel.Address = "Cargando...";
				addressTextField.Editable = false;
				addressTextField.ResignFirstResponder();
			};
			mapView.RegionChanged += (object sender, MKMapViewChangeEventArgs e) => {
				addressTextField.Editable = true;
				var mapViewParam = (MKMapView)sender;
				cllocation = new CLLocation(mapViewParam.Region.Center.Latitude, mapViewParam.Region.Center.Longitude);
				clg.ReverseGeocodeLocation(cllocation, (CLPlacemark[] placemarks, NSError error)=>{
					if (placemarks == null)
						return;
					for(var i = 0; i < placemarks.Length; i++)
					{
						var loc = placemarks[i];
						address = String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
							loc.Name, loc.SubLocality, loc.Locality,
							loc.AdministrativeArea, loc.Country, loc.PostalCode); 
						isoCountryCode = loc.IsoCountryCode;
						viewModel.Address = address;
					}
				});
			};

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender,args) => {

					var fullNameString = fullNameTextField.Text.Trim ();
					var phoneNumberString = phoneNumberTextField.Text.Trim ();
					var emailString = emailTextField.Text.Trim ();
					var addressString = addressTextField.Text.Trim();

					//Check if data is well formatted, otherwise publish data to ViewModel
					if (fullNameString.Equals("")){
						var alert = new UIAlertView("¿A quién vamos a entregar?", "Favor de escribir el nombre del destinatario"
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								fullNameTextField.BecomeFirstResponder();
						};
						alert.Show();
					}else

					if (phoneNumberString.Equals ("") || phoneNumberString.Length < 8){
						var alert = new UIAlertView("Favor de escribir un número celular válido", ""
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								phoneNumberTextField.BecomeFirstResponder();
						};
						alert.Show();
					}else

					if (emailString.Equals("")|| !StringValidator.IsValidEmail(emailString)){
						var alert = new UIAlertView("Ingresa un correo válido", ""
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								emailTextField.BecomeFirstResponder();
						};
						alert.Show();
					}else

					if (addressString.Equals("Cargando...") || addressString.Equals("") ){
						var alert = new UIAlertView("Ingresa una dirección válida", ""
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								addressTextField.BecomeFirstResponder();
						};
						alert.Show();
					}else{
						viewModel.PublishData();
						NavigationController.PopViewControllerAnimated(true);
					}

				})
				, true);
		}
	}
}