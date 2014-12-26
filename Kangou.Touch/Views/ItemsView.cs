using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using SlidingPanels.Lib;
using MonoTouch.Dialog;
using System;

namespace Kangou.Touch.Views
{
	[Register("ItemsView")]
	public class ItemsView : MvxViewController
	{
		UITextView itemsTextField;

		public override void ViewDidLoad()
		{
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			ItemsViewModel viewModel = (ItemsViewModel)ViewModel;
			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH * 0.1f;

			var PADDING_BTWN_ELEMENT = MARGIN_SUBVIEWS;
			var HEIGHT_LABEL = HEIGHT * 0.05f;
			var HEIGHT_TEXT_FIELD = HEIGHT * 0.14f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			var pYoffset = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height + MARGIN_SUBVIEWS * 0.5f;

			//Info Pick Up with Purchase
			var infoPickUpLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_LABEL));
			infoPickUpLabel.Text = "¿Recoger o Comprar?";
			infoPickUpLabel.Lines = 0;
			infoPickUpLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			infoPickUpLabel.TextColor = UIColor.DarkGray;
			infoPickUpLabel.TextAlignment = UITextAlignment.Center;
			infoPickUpLabel.Alpha = 0.75f;
			Add (infoPickUpLabel);

			//Segmented Control Is It Purchase
			pYoffset += PADDING_BTWN_ELEMENT;
			var segmentControl = new UISegmentedControl();
			var heightSegmentControl = HEIGHT_LABEL;
			segmentControl.Frame = new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2,heightSegmentControl);
			segmentControl.InsertSegment("Recoger", 0, true);
			segmentControl.InsertSegment("Comprar", 1, true);
			segmentControl.SelectedSegment = -1;
			segmentControl.TintColor = UIColor.LightGray;
			segmentControl.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.5f);
			segmentControl.ValueChanged += (sender, e) => {
				var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
				viewModel.IsAPurchase = selectedSegmentId == 1;
			};
			Add (segmentControl);
			pYoffset += heightSegmentControl + PADDING_BTWN_ELEMENT * 0.5f;

			//Info Pick Up Label
			var infoLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_LABEL));
			infoLabel.Text = "Inserta aquí el/los producto(s)";
			infoLabel.Lines = 0;
			infoLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			infoLabel.TextColor = UIColor.DarkGray;
			infoLabel.TextAlignment = UITextAlignment.Center;
			infoLabel.Alpha = 0.75f;
			Add (infoLabel);
			pYoffset += PADDING_BTWN_ELEMENT;

			//Text Field Pick Up Label
			itemsTextField = new UITextView(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_TEXT_FIELD));
			itemsTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsTextField.TextAlignment = UITextAlignment.Center;
			itemsTextField.Layer.CornerRadius = 0.5f;
			itemsTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			itemsTextField.Layer.BorderWidth = 0.5f;
			itemsTextField.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.5f);
			Add(itemsTextField);
			var tapGesture = new UITapGestureRecognizer ((g) => {
				itemsTextField.ResignFirstResponder();
			});
			View.AddGestureRecognizer (tapGesture);

			//View Model Binding
			var set = this.CreateBindingSet<ItemsView, ItemsViewModel>();
			set.Bind(itemsTextField).To(vm => vm.Items);
			set.Apply();

			//Add Button
			var titleForRightButton = "Guardar";
			if(RegisterOrderViewModel.isStraightNavigation)
				titleForRightButton = "Continuar";

			var rightButton = new UIBarButtonItem (titleForRightButton, UIBarButtonItemStyle.Done, (sender, args) => {
				if (itemsTextField.Text.ToString ().Trim ().Equals ("")) {    
					new UIAlertView ("¿Olvidaste, apeteces o necesitas algo?", "Favor de escribir sus productos"
						, null, "Ok", null).Show ();

				} else 
				if(segmentControl.SelectedSegment <0) {
					new UIAlertView ("Favor de indicar si hay que Recoger o Comprar", ""
						, null, "Ok", null).Show ();
				} else {
					viewModel.PublishData ();
				}
			});
			NavigationItem.SetRightBarButtonItem(rightButton, true);
			NavigationItem.Title = "1. Productos";
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

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			itemsTextField.BecomeFirstResponder ();
		}
			
	}
}