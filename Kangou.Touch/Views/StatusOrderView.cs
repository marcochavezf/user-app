using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;
using System.Threading.Tasks;
using System.Threading;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using Kangou.Core;
using Kangou.Touch;
using System.Diagnostics;
using SlidingPanels.Lib;
using Kangou.Core.Helpers;

namespace KangouMessenger.Touch
{
	[Register("PickUpRouteView")]
	public class StatusOrderView : MvxViewController
    {
		private StatusOrderViewModel _viewModel;
	
        public override void ViewDidLoad()
        {
			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;

			//Load Root View
			base.ViewDidLoad();
		
			//Setting origin and destiny directions
			var origin = new CLLocationCoordinate2D (19.43260,-99.13320);
			_viewModel = (StatusOrderViewModel)ViewModel;

			//Adding map
			var widthMap = CONTAINER_SIZE.Width;
			var heightMap = CONTAINER_SIZE.Height;
			var mapView = new MKMapView ();
			mapView.SetRegion (MKCoordinateRegion.FromDistance (origin, 1000, 1000), true);
			mapView.Frame = new RectangleF (0, 0, widthMap, heightMap);
			mapView.PitchEnabled = true;
			mapView.ShowsBuildings = true;
			mapView.ShowsUserLocation = true;
			Add (mapView);

			var annotation = new MKPointAnnotation () {
				Title = "Kangou"
			};
			mapView.AddAnnotation (annotation);

			//Status Text View
			var widthTextView = WIDTH; 
			var heightTextView = Constants.HEIGHT_TEXTVIEW * 1.25f;
			var posYinst = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height;
			var posXOffsetInst = 0f;
			var alphaTextViews = 0.75f;
			var statusTextView = new UITextView(new RectangleF(posXOffsetInst, posYinst, widthTextView, heightTextView));
			statusTextView.Editable = false;
			statusTextView.Font = UIFont.FromName(Constants.LABEL_BOLD_FONT, Constants.LABEL_FONT_SIZE);
			statusTextView.TextAlignment = UITextAlignment.Center;
			statusTextView.Alpha = alphaTextViews;
			Add(statusTextView);

			//Distance Text View
			var posYbutton = heightMap - heightTextView;
			var distanceTextView = new UITextView(new RectangleF(posXOffsetInst, posYbutton, widthTextView, heightTextView));
			distanceTextView.Editable = false;
			distanceTextView.Font = UIFont.FromName(Constants.LABEL_BOLD_FONT, Constants.LABEL_FONT_SIZE);
			distanceTextView.TextAlignment = UITextAlignment.Center;
			distanceTextView.Alpha = alphaTextViews;
			Add(distanceTextView);

			//Binding
			var set = this.CreateBindingSet<StatusOrderView, StatusOrderViewModel>();
			set.Bind(statusTextView).To(vm => vm.Status);
			set.Bind(distanceTextView).To(vm => vm.Distance);
			set.Apply();

			ConnectionManager.On (SocketEvents.KangouPosition, (data) => {
				var lat = Convert.ToDouble(data["lat"]);
				var lng = Convert.ToDouble(data["lng"]);

				CLLocation destiny;
				double meters;
				double km;

				var coordinate = new CLLocationCoordinate2D(lat, lng);
				InvokeOnMainThread(delegate {
					annotation.Coordinate = coordinate;
					mapView.SetCenterCoordinate(coordinate, true);

					switch (_viewModel.ActiveOrder.Status) {
					case StatusOrder.KangouGoingToPickUp:
						destiny = new CLLocation(_viewModel.ActiveOrder.PickUpLat, _viewModel.ActiveOrder.PickUpLng);
						meters = destiny.DistanceFrom(new CLLocation(lat,lng));
						km = Math.Round(meters/1000.0f, 1);
						distanceTextView.Text = "El kangou está a " + km + " Km para recoger o comprar";
						break;

					case StatusOrder.KangouGoingToDropOff:
						destiny = new CLLocation(_viewModel.ActiveOrder.DropOffLat, _viewModel.ActiveOrder.DropOffLng);
						meters = destiny.DistanceFrom(new CLLocation(lat,lng));
						km = Math.Round(meters/1000.0f, 1);
						distanceTextView.Text = "El kangou está a " + km + " Km para entregar";
						break;
					}

				});
			});

			Console.WriteLine (_viewModel.ActiveOrder.Status);

			if(_viewModel.ActiveOrder.Status == StatusOrder.SearchingForKangou)
				new UIAlertView ("Tu orden ha sido confirmada, estamos buscando al kangou más cercano para brindarte servicio.", "", null, "Ok").Show();
				
			if (_viewModel.ActiveOrder.Status == StatusOrder.OrderReviewed) {
				var orderFinishedAlert = new UIAlertView ("Esta orden ya ha finalizado", "¡Muchas gracias por usar Kangou!", null, "Ok");
				orderFinishedAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
					StatusOrderViewModel.HasBeenClosedByUser = true;
					ConnectionManager.Disconnect();
					NavigationController.PopViewControllerAnimated(true);
				};
				orderFinishedAlert.Show ();
			}

			_viewModel.StatusUpdated += (message) => {
				Console.WriteLine("Scheduling Local Notification: {0}",message);
				InvokeOnMainThread (delegate {  
					ScheduleStatusOrderNotification(message);
				});
			};
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
			if (_viewModel.ActiveOrder.Status != StatusOrder.OrderSignedByClient) {
				//It's dissapearing because user has pressed back button
				StatusOrderViewModel.HasBeenClosedByUser = true;
				ConnectionManager.Disconnect ();
			}
			_viewModel.TurnOffConnectionManager ();
		}


		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			Console.WriteLine ("ViewWillDisappear Status");
		}

		private void ScheduleStatusOrderNotification(string message){
			var notification = new UILocalNotification();
			notification.FireDate = DateTime.Now;
			notification.AlertAction = "Aviso Kangou";
			notification.AlertBody = message;
			notification.ApplicationIconBadgeNumber = 1;
			notification.SoundName = UILocalNotification.DefaultSoundName;
			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}
    }
}