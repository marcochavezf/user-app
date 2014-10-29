using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using SlidingPanels.Lib;

namespace Kangou.Touch.Views
{
	[Register("ItemsView")]
	public class ItemsView : MvxViewController
	{
		UITextField itemsTextField;

		public override void ViewDidLoad()
		{
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH * 0.1f; 

			var PADDING_BTWN_ELEMENT = MARGIN_SUBVIEWS;
			var HEIGHT_TEXTVIEWS = HEIGHT * 0.2f;
			var HEIGHT_LABEL = HEIGHT * 0.125f;

			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			var pYoffset = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height + MARGIN_SUBVIEWS;

			//Items Text Field
			itemsTextField = new UITextField(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_TEXTVIEWS));
			itemsTextField.Placeholder = "Agrega aquí el/los producto(s)";
			itemsTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsTextField.TextColor = UIColor.Black;
			itemsTextField.TextAlignment = UITextAlignment.Center;
			itemsTextField.Layer.BorderColor = UIColor.DarkGray.CGColor;
			itemsTextField.Layer.BorderWidth = 0.5f;
			itemsTextField.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.5f);
			View.Add(itemsTextField);
			pYoffset += PADDING_BTWN_ELEMENT;

			//Info Pick Up Label
			pYoffset += HEIGHT_LABEL + PADDING_BTWN_ELEMENT;
			var infoPickUpLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_LABEL));
			infoPickUpLabel.Text = "Escribe en la parte de arriba los\nproductos a traer o comprar.";
			infoPickUpLabel.Lines = 0;
			infoPickUpLabel.Font = UIFont.FromName(LABEL_FONT, 12f);
			infoPickUpLabel.TextColor = UIColor.Black;
			infoPickUpLabel.TextAlignment = UITextAlignment.Center;
			infoPickUpLabel.BackgroundColor = UIColor.FromWhiteAlpha (1f, 0.9f);
			infoPickUpLabel.Layer.BorderColor = UIColor.DarkGray.CGColor;
			infoPickUpLabel.Layer.BorderWidth = 0.5f;
			infoPickUpLabel.Alpha = 0.75f;
			Add (infoPickUpLabel);

			var tapGesture = new UITapGestureRecognizer ((g) => {
				itemsTextField.ResignFirstResponder();
			});
			View.AddGestureRecognizer (tapGesture);

			//View Model Binding
			var set = this.CreateBindingSet<ItemsView, ItemsViewModel>();
			set.Bind(itemsTextField).To(vm => vm.Items);
			set.Apply();

			//Add Button
			var rightButton = new UIBarButtonItem ("Guardar", UIBarButtonItemStyle.Done, (sender, args) => {
				if (itemsTextField.Text.ToString ().Trim ().Equals ("")) {    
					new UIAlertView ("¿Olvidaste, apeteces o necesitas algo?", "Favor de escribir sus productos"
						, null, "Ok", null).Show ();

				} else {
					var viewModel = (ItemsViewModel)ViewModel;
					viewModel.PublishData ();
					NavigationController.PopViewControllerAnimated (true);
				}
			});
			NavigationItem.SetRightBarButtonItem(rightButton, true);
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