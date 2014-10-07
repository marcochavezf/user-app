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
			var HEIGHT_TEXTVIEW = 67.5f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			var viewModel = (PickUpViewModel)ViewModel;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;
				
			var pYoffset = MARGIN_HEIGHT_SUBVIEWS;

			//Pick Up Address Text View
			var addressTextView = new UITextView(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTVIEW));
			addressTextView.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			addressTextView.Layer.BorderWidth = 0.5f;
			addressTextView.Layer.BorderColor = UIColor.Gray.CGColor;
			addressTextView.TextAlignment = UITextAlignment.Center;
			Add (addressTextView);
			pYoffset += HEIGHT_TEXTVIEW + MARGIN_HEIGHT_SUBVIEWS;

			//Toolbar with Done Button
			var toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			addressTextView.InputAccessoryView = toolbar;
			toolbar.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					addressTextView.ResignFirstResponder();
				})
			};

			//Binding
			var set = this.CreateBindingSet<PickUpView, PickUpViewModel>();
			set.Bind(addressTextView).To(vm => vm.Address);
			set.Apply();

			//Adding map
			var mapView = new MKMapView ();
			mapView.SetRegion (MKCoordinateRegion.FromDistance (new CLLocationCoordinate2D (19.430566, -99.200946), 1000, 1000), true);
			mapView.Frame = new RectangleF (0, pYoffset, CONTAINER_SIZE.Width, CONTAINER_SIZE.Height - pYoffset);
			mapView.ShowsBuildings = true;
			mapView.PitchEnabled = true;
			mapView.ShowsUserLocation = true;
			var tapGesture = new UITapGestureRecognizer ((g) => {
				addressTextView.ResignFirstResponder();
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
				addressTextView.Editable = false;
				addressTextView.ResignFirstResponder();
			};
			mapView.RegionChanged += (object sender, MKMapViewChangeEventArgs e) => {
				addressTextView.Editable = true;
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

					if(viewModel.Address.Trim().Contains("Cargando...")){
						var alert = new UIAlertView("Ingresa una dirección válida", ""
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								addressTextView.BecomeFirstResponder();
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