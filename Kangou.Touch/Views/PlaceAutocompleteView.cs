using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Kangou.Core.ViewModels;
using System;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Kangou.Helpers;
using System.Collections.Generic;

namespace Kangou.Touch.Views
{
	[Register("PlacesListView")]
	public class PlaceAutocompleteView : MvxViewController
	{
		private UITextField _textField;

		public override void ViewDidLoad()
		{
			View = new UIView (){ BackgroundColor = UIColor.White };
			base.ViewDidLoad();

			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var HEIGHT_TEXTFIELD = Constants.HEIGHT_TEXTFIELD;
			var posYoffset = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height + HEIGHT_TEXTFIELD * 0.25f;

			//Adding Text Field
			var posXoffset = HEIGHT_TEXTFIELD * 0.38f;
			_textField = new UITextField(new RectangleF(posXoffset, posYoffset, WIDTH-posXoffset, HEIGHT_TEXTFIELD));
			_textField.Placeholder = "Ingresa la ubicación";
			Add(_textField);
			posYoffset += HEIGHT_TEXTFIELD;
			var gesture = new UITapGestureRecognizer ((g)=> {
				_textField.ResignFirstResponder();
			});
			gesture.CancelsTouchesInView = false;

			//Adding Google Top Logo
			var googleLogoTop = new UIImageView (UIImage.FromBundle("powered-by-google.png"));
			var widthLogo = 150f;
			var heightLogo = 23f;
			googleLogoTop.Frame = new RectangleF (posXoffset-1f, posYoffset + heightLogo * 0.75f, widthLogo, heightLogo);
			Add (googleLogoTop);

			//Creating Table
			var heightTable = HEIGHT * 0.65f;
			var tableView = new UITableView(new RectangleF(0, posYoffset, WIDTH, heightTable), UITableViewStyle.Plain);
			var source = new MvxStandardTableViewSource(tableView, "TitleText description");
			tableView.Source = source;
			tableView.AddGestureRecognizer (gesture);
			Add(tableView);
			posYoffset += heightTable;

			//Adding Google Bottom Logo
			var googleLogoBottom = new UIImageView (UIImage.FromBundle("powered-by-google.png"));
			googleLogoBottom.Frame = new RectangleF (posXoffset-1f, posYoffset + heightLogo * 0.75f, widthLogo, heightLogo);
			Add (googleLogoBottom);

			//Data Binding
			var set = this.CreateBindingSet<PlaceAutocompleteView, PlaceAutocompleteViewModel>();
			set.Bind(_textField).To(vm => vm.InputToPredict);
			set.Bind(source).To(vm => vm.PlacesList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectPlaceCommand);
			set.Bind(tableView).For("Visibility").To(vm => vm.IsTableVisible).WithConversion("Visibility");
			set.Bind(googleLogoBottom).For("Visibility").To(vm => vm.IsTableVisible).WithConversion("Visibility");
			set.Bind(googleLogoTop).For("Visibility").To(vm => vm.IsTableVisible).WithConversion("InvertedVisibility");
			set.Apply();

			NavigationItem.Title = "Buscar Lugar";
			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Regresar", UIBarButtonItemStyle.Plain, null); 
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_SECONDARY;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			_textField.BecomeFirstResponder ();

			NavigationItem.HidesBackButton = false;
			SlidingGestureRecogniser.EnableGesture = false;
		}
	}
}