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

namespace Kangou.Touch.Views
{
	[Register("PickUpView")]
	public class PickUpView : MvxViewController
	{
		public override void ViewDidLoad()
		{
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH / 8 * 0.5f; 
			var MARGIN_HEIGHT_SUBVIEWS = MARGIN_WIDTH_SUBVIEWS;
			var HEIGHT_TEXTFIELD = HEIGHT * 0.075f;
			var HEIGHT_TEXTVIEW = HEIGHT * 0.075f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			var viewModel = (PickUpViewModel)ViewModel;

			var pYoffset = HEIGHT * 0.175f;

			//FullName
			var fullNameTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			fullNameTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			fullNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			fullNameTextField.Layer.BorderWidth = 0.5f;
			fullNameTextField.TextAlignment = UITextAlignment.Center;
			fullNameTextField.Placeholder = "Nombre de la persona o negocio";
			Add (fullNameTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//Email
			var referencesTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			referencesTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			referencesTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			referencesTextField.Layer.BorderWidth = 0.5f;
			referencesTextField.TextAlignment = UITextAlignment.Center;
			referencesTextField.Placeholder = "Referencias (puerta azul, esquina...)";
			Add (referencesTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//DropOff Address Text Field
			var addressTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTVIEW));
			addressTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			addressTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			addressTextField.Layer.BorderWidth = 0.5f;
			addressTextField.TextAlignment = UITextAlignment.Center;
			addressTextField.Enabled = false;
			Add (addressTextField);
			pYoffset += HEIGHT_TEXTVIEW + MARGIN_HEIGHT_SUBVIEWS;

			//Toolbar with Done Button for FullName
			var toolbarFullName = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarFullName.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					referencesTextField.BecomeFirstResponder();
				})
			};
			fullNameTextField.InputAccessoryView = toolbarFullName;

			//Toolbar with Done Button for PhoneNumber
			var toolbarReferences = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarReferences.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					referencesTextField.ResignFirstResponder();
				})
			};
			referencesTextField.InputAccessoryView = toolbarReferences;

			//Binding
			var set = this.CreateBindingSet<PickUpView, PickUpViewModel>();
			set.Bind(fullNameTextField).To(vm => vm.FullName);
			set.Bind(referencesTextField).To(vm => vm.References);
			set.Bind(addressTextField).To(vm => vm.AddressToDisplay);
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
			viewModel.AddressToDisplay = "Cargando...";
			pYoffset += mapView.Frame.Height * 0.5f;

			//Adding Pin
			var kangouMarker = new UIImageView (UIImage.FromBundle("ic_marker.png"));
			var widthMarker = 50f;
			var heightMarker = 60f;
			kangouMarker.Frame = new RectangleF (CONTAINER_SIZE.Width*0.5f - widthMarker*0.5f, pYoffset - heightMarker*1.525f, widthMarker, heightMarker);
			Add (kangouMarker);

			//Getting variables to set address
			var clg = new CLGeocoder();
			var addressToDisplay = "";
			var cllocation = new CLLocation(mapView.Region.Center.Latitude, mapView.Region.Center.Longitude);

			//Getting address first time
			clg.ReverseGeocodeLocation(cllocation, (CLPlacemark[] placemarks, NSError error)=>{
				if (placemarks == null)
					return;
				for(var i = 0; i < placemarks.Length; i++)
				{
					var loc = placemarks[i];

					viewModel.Street = loc.Name;
					viewModel.SubLocality = loc.SubLocality;
					viewModel.Locality = loc.Locality;
					viewModel.AdministrativeArea = loc.AdministrativeArea;
					viewModel.Country = loc.Country;
					viewModel.PostalCode = loc.PostalCode;
					viewModel.IsoCountryCode = loc.IsoCountryCode;
					viewModel.Lat = cllocation.Coordinate.Latitude;
					viewModel.Lng = cllocation.Coordinate.Longitude;

					addressToDisplay = String.Format("{0}, {1}", loc.Name, loc.SubLocality); 
					if(addressToDisplay.Length > 38)
						addressToDisplay = addressToDisplay.Substring(0,34) + "...";
					viewModel.AddressToDisplay = addressToDisplay;
					Console.WriteLine("ShortAddress: {0} Lat:{1}, Lng:{2}",addressToDisplay,viewModel.Lat,viewModel.Lng);
				}
			});

			//Getting address when user move region map
			mapView.RegionWillChange += (object sender, MKMapViewChangeEventArgs e) => {
				viewModel.AddressToDisplay = "Cargando...";
			};
			mapView.RegionChanged += (object sender, MKMapViewChangeEventArgs e) => {
				var mapViewParam = (MKMapView)sender;
				cllocation = new CLLocation(mapViewParam.Region.Center.Latitude, mapViewParam.Region.Center.Longitude);
				clg.ReverseGeocodeLocation(cllocation, (CLPlacemark[] placemarks, NSError error)=>{
					if (placemarks == null)
						return;
					for(var i = 0; i < placemarks.Length; i++)
					{
						var loc = placemarks[i];

						viewModel.Street = loc.Name;
						viewModel.SubLocality = loc.SubLocality;
						viewModel.Locality = loc.Locality;
						viewModel.AdministrativeArea = loc.AdministrativeArea;
						viewModel.Country = loc.Country;
						viewModel.PostalCode = loc.PostalCode;
						viewModel.IsoCountryCode = loc.IsoCountryCode;
						viewModel.Lat = cllocation.Coordinate.Latitude;
						viewModel.Lng = cllocation.Coordinate.Longitude;

						addressToDisplay = String.Format("{0}, {1}", loc.Name, loc.SubLocality); 
						if(addressToDisplay.Length > 38)
							addressToDisplay = addressToDisplay.Substring(0,34) + "...";
						viewModel.AddressToDisplay = addressToDisplay;
						Console.WriteLine("ShortAddress: {0} Lat:{1}, Lng:{2}",addressToDisplay,viewModel.Lat,viewModel.Lng);
					}
				});
			};

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender,args) => {

					if(viewModel.AddressToDisplay.Trim().Contains("Cargando...")){
						var alert = new UIAlertView("Ingresa una dirección válida", ""
							, null, "Ok", null);
						alert.Show();
					} else 

						if(String.IsNullOrEmpty( viewModel.References )){
							var alert = new UIAlertView("Ingresa al menos una referencia", "\nEjemplo: el color de la casa, entre que calles se encuentra, etc."
								, null, "Ok", null);
							alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
								if(e.ButtonIndex == 0)
									referencesTextField.BecomeFirstResponder();
							};
							alert.Show();
						}

					 else{
						viewModel.PublishData();
						NavigationController.PopViewControllerAnimated(true);
					}
				})
				, true);
		}
	}
}