using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;
using KangouMessenger.Core;
using System.Threading.Tasks;
using System.Threading;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using Kangou.Core;
using Kangou.Touch;
using System.Diagnostics;

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
			var origin = new CLLocationCoordinate2D (19.4361727,-99.1954972);
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

			//Address Text View
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

			//Help Button
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

			Console.WriteLine (_viewModel.ActiveOrder.Status);

			if(_viewModel.ActiveOrder.Status == StatusOrder.SearchingForKangou)
				new UIAlertView ("Tu orden ha sido confirmada, estamos buscando al kangou más cercano para brindarte servicio.", "", null, "Ok").Show();
				
			if (_viewModel.ActiveOrder.Status == StatusOrder.OrderReviewed) {
				var orderFinishedAlert = new UIAlertView ("Esta orden ya ha finalizado", "¡Muchas gracias por usar Kangou!", null, "Ok");
				orderFinishedAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
					ConnectionManager.Disconnect();
					NavigationController.PopViewControllerAnimated(true);
				};
				orderFinishedAlert.Show ();
			}
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			if (_viewModel.ActiveOrder.Status != StatusOrder.OrderSignedByClient) {
				ConnectionManager.Disconnect ();
			}
			_viewModel.TurnOffConnectionManager ();
		}
    }
}