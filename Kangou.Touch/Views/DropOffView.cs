using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using MonoTouch.CoreLocation;
using System;
using SlidingPanels.Lib;
using Google.Maps;
using Kangou.Core;

namespace Kangou.Touch.Views
{
	[Register("DropOffViewView")]
	public class DropOffView : BusyMvxViewController
	{
		public static bool HasBeenOpenedFromList = false;
		private MapView _mapView;
		private volatile bool _isAddressUpdatedByAutocomplete = false;

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
			var LIMIT_CHARS_SHORT_ADDRESS = 35;

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			var viewModel = (DropOffViewModel)ViewModel;
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			var clg = new CLGeocoder();

			var pYoffset = HEIGHT * 0.175f;

			//FullName
			var fullNameTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			fullNameTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			fullNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			fullNameTextField.Layer.BorderWidth = 0.5f;
			fullNameTextField.TextAlignment = UITextAlignment.Center;
			fullNameTextField.Placeholder = "Nombre de la persona o negocio";
			fullNameTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (fullNameTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//References
			var referencesTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			referencesTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			referencesTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			referencesTextField.Layer.BorderWidth = 0.5f;
			referencesTextField.TextAlignment = UITextAlignment.Center;
			referencesTextField.Placeholder = "Referencias (puerta azul, esquina...)";
			referencesTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (referencesTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;


			//DropOff Address Text Field
			var addressTextField = new UIButton (UIButtonType.RoundedRect);
			addressTextField.SetTitle ("Cargando...", UIControlState.Normal);
			addressTextField.SetTitleColor (UIColor.DarkTextColor, UIControlState.Highlighted);
			addressTextField.SetTitleColor (UIColor.DarkTextColor, UIControlState.Normal);
			addressTextField.SetTitleColor (UIColor.DarkTextColor, UIControlState.Selected);
			addressTextField.SetTitleColor (UIColor.DarkTextColor, UIControlState.Application);
			addressTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			addressTextField.TintColor = UIColor.DarkTextColor;
			addressTextField.Frame = new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTVIEW);
			addressTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			addressTextField.Layer.BorderWidth = 0.5f;
			addressTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (addressTextField);
			pYoffset += HEIGHT_TEXTVIEW + MARGIN_HEIGHT_SUBVIEWS;


			var tapGesture = new UITapGestureRecognizer ((g) => {
				fullNameTextField.ResignFirstResponder();
				referencesTextField.ResignFirstResponder();
			});
			View.AddGestureRecognizer (tapGesture);

			//Binding
			var set = this.CreateBindingSet<DropOffView, DropOffViewModel>();
			set.Bind(fullNameTextField).To(vm => vm.FullName);
			set.Bind(referencesTextField).To(vm => vm.References);
			set.Bind(addressTextField).For("Title").To(vm => vm.AddressToDisplay);
			set.Bind(addressTextField).To(vm => vm.OpenPlaceAutocompleteCommand);
			set.Apply();

			//Adding map
			var camera = CameraPosition.FromCamera (
				latitude: RegisterOrderViewModel.LOCATION.Lat, 
				longitude: RegisterOrderViewModel.LOCATION.Lng, 
				zoom: 15);
			_mapView = MapView.FromCamera (RectangleF.Empty, camera);
			_mapView.Frame = new RectangleF (0, pYoffset, CONTAINER_SIZE.Width, CONTAINER_SIZE.Height - pYoffset);
			_mapView.Layer.BorderColor = UIColor.Gray.CGColor;
			_mapView.Layer.BorderWidth = 0.5f;
			InvokeOnMainThread (()=> _mapView.MyLocationEnabled = true);
			Add (_mapView);
			viewModel.AddressToDisplay = "Cargando...";
			pYoffset += _mapView.Frame.Height * 0.5f;
			viewModel.ChangeLocation = (placeDetailsResponse) => {

				_isAddressUpdatedByAutocomplete = true;
				var cllocationLocal = new CLLocationCoordinate2D(
					placeDetailsResponse.result.geometry.location.Lat, 
					placeDetailsResponse.result.geometry.location.Lng);

				InvokeOnMainThread(delegate {
					_mapView.Camera = new CameraPosition(cllocationLocal,15,0,0);

					viewModel.Street = "";
					viewModel.SubLocality = "";
					viewModel.AdministrativeArea = "";

					foreach(var addressComponent in placeDetailsResponse.result.address_components)
					{
						if(addressComponent.types.Contains("street_address")
							|| addressComponent.types.Contains("route") ){
							if(String.IsNullOrWhiteSpace(viewModel.Street))
								viewModel.Street = addressComponent.long_name;
							else
								viewModel.Street = addressComponent.long_name +" "+ viewModel.Street;
						}

						else
							if(addressComponent.types.Contains("street_number") ){
								if(String.IsNullOrWhiteSpace(viewModel.Street))
									viewModel.Street = addressComponent.long_name;
								else
									viewModel.Street = viewModel.Street +" "+addressComponent.long_name;
							}

							else
								if(addressComponent.types.Contains("sublocality") 
									|| addressComponent.types.Contains("neighborhood") ){
									if(String.IsNullOrWhiteSpace(viewModel.SubLocality))
										viewModel.SubLocality = addressComponent.long_name;
									else
										viewModel.SubLocality = viewModel.SubLocality +", "+addressComponent.long_name;
								}

								else
									if(addressComponent.types.Contains("locality")){
										viewModel.Locality = addressComponent.long_name;
									}

									else
										if(addressComponent.types.Contains("dministrative_area_level_1") 
											|| addressComponent.types.Contains("dministrative_area_level_2") ){
											if(String.IsNullOrWhiteSpace(viewModel.AdministrativeArea))
												viewModel.AdministrativeArea = addressComponent.long_name;
											else
												viewModel.AdministrativeArea = viewModel.AdministrativeArea +" "+addressComponent.long_name;
										}	

										else
											if(addressComponent.types.Contains("country")){
												viewModel.Country = addressComponent.long_name;
												viewModel.IsoCountryCode = addressComponent.short_name;
											}

											else
												if(addressComponent.types.Contains("postal_code")){
													viewModel.PostalCode = addressComponent.long_name;
												}
					}

					viewModel.Lat = cllocationLocal.Latitude;
					viewModel.Lng = cllocationLocal.Longitude;

					String addressToDisplayAutocomplete; 
					if(String.IsNullOrWhiteSpace(viewModel.Street))
						addressToDisplayAutocomplete = viewModel.SubLocality;
					else if (String.IsNullOrWhiteSpace(viewModel.SubLocality))
						addressToDisplayAutocomplete = viewModel.Street;
					else
						addressToDisplayAutocomplete = String.Format("{0}, {1}", viewModel.Street, viewModel.SubLocality); 
					if(addressToDisplayAutocomplete.Length > LIMIT_CHARS_SHORT_ADDRESS)
						addressToDisplayAutocomplete = addressToDisplayAutocomplete.Substring(0,LIMIT_CHARS_SHORT_ADDRESS-4) + "...";
					viewModel.AddressToDisplay = addressToDisplayAutocomplete;
					Console.WriteLine("ShortAddress: {0} Lat:{1}, Lng:{2}",addressToDisplayAutocomplete, viewModel.Lat,viewModel.Lng);
				});
			};

			//Adding Pin
			var kangouMarker = new UIImageView (UIImage.FromBundle("ic_marker.png"));
			var widthMarker = 41f;
			var heightMarker = 60f;
			kangouMarker.Frame = new RectangleF (CONTAINER_SIZE.Width*0.5f - widthMarker*0.5f, pYoffset - heightMarker*1.0f, widthMarker, heightMarker);
			Add (kangouMarker);


			//Getting variables to set address
			var cllocation = new CLLocation(_mapView.Camera.Target.Latitude, _mapView.Camera.Target.Longitude);
			var addressToDisplay = "";
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

					if(String.IsNullOrWhiteSpace(viewModel.Street))
						addressToDisplay = viewModel.SubLocality;
					else if (String.IsNullOrWhiteSpace(viewModel.SubLocality))
						addressToDisplay = viewModel.Street;
					else
						addressToDisplay = String.Format("{0}, {1}", viewModel.Street, viewModel.SubLocality); 
					if(addressToDisplay.Length > LIMIT_CHARS_SHORT_ADDRESS)
						addressToDisplay = addressToDisplay.Substring(0,LIMIT_CHARS_SHORT_ADDRESS-4) + "...";
					viewModel.AddressToDisplay = addressToDisplay;
					Console.WriteLine("ShortAddress: {0} Lat:{1}, Lng:{2}",addressToDisplay,viewModel.Lat,viewModel.Lng);
				}
			});

			//Getting address when user move region map
			_mapView.CameraPositionChanged += (object sender, GMSCameraEventArgs e) => {

				if(_isAddressUpdatedByAutocomplete){
					_isAddressUpdatedByAutocomplete = false;
					return;
				}

				viewModel.AddressToDisplay = "Cargando...";
				var mapViewParam = (MapView)sender;

				cllocation = new CLLocation(mapViewParam.Camera.Target.Latitude, mapViewParam.Camera.Target.Longitude);
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

						if(String.IsNullOrWhiteSpace(viewModel.Street))
							addressToDisplay = viewModel.SubLocality;
						else if (String.IsNullOrWhiteSpace(viewModel.SubLocality))
							addressToDisplay = viewModel.Street;
						else
							addressToDisplay = String.Format("{0}, {1}", viewModel.Street, viewModel.SubLocality); 
						if(addressToDisplay.Length > LIMIT_CHARS_SHORT_ADDRESS)
							addressToDisplay = addressToDisplay.Substring(0,LIMIT_CHARS_SHORT_ADDRESS-4) + "...";
						viewModel.AddressToDisplay = addressToDisplay;
						Console.WriteLine("ShortAddress: {0} Lat:{1}, Lng:{2}",addressToDisplay,viewModel.Lat,viewModel.Lng);
					}
				});
			};

			var titleForRightButton = "Guardar";
			if(RegisterOrderViewModel.isStraightNavigation)
				titleForRightButton = "Continuar";

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(titleForRightButton, UIBarButtonItemStyle.Done, (sender,args) => {

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

						else

							if(!ContainSomeDigit(viewModel.Street)){
								var alert = new UIAlertView("Dirección sin número", "\nLa dirección debe de contener el número exterior."
									, null, "Ok", null);
								alert.Show();
							} else 
								if(!GpsArea.IsWithinSomeAvailableArea(viewModel.Lat, viewModel.Lng)){
									var alert = new UIAlertView("Fuera del área de servicio", "\nLo sentimos todavía no hay Kangous disponibles en esta zona :("
										, null, "Ok", null);
									alert.Show();
								} else {
								viewModel.PublishData();
							}
				})
				, true);
			if(!RegisterOrderViewModel.isStraightNavigation)
				NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Cancelar", UIBarButtonItemStyle.Plain, null); 
			NavigationItem.Title = "3. Entregar";
		}

		public bool ContainSomeDigit(string s)
		{
			foreach (char c in s)
			{
				if (Char.IsDigit(c))
					return true;
			}
			return false;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			SlidingGestureRecogniser.EnableGesture = false;
		}

		public override void ViewDidAppear (bool animated)
		{
			if (HasBeenOpenedFromList) {
				HasBeenOpenedFromList = false;
				_popNextToLastViewController = true;
				base.ViewDidAppear (animated);
			}
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			SlidingGestureRecogniser.EnableGesture = true;

			_popNextToLastViewController = false;
			_hideBackButton = false;
		}
	}
}