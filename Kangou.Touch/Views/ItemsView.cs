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

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH / 8; 
			var MARGIN_TEXTFIELD = WIDTH * 0.05f;

			var PADDING_BTWN_ELEMENT = HEIGHT * 0.2f;
			var HEIGHT_TEXTVIEWS = HEIGHT * 0.2f;

			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			var pYoffset = HEIGHT * 0.05f;

			//Info Pick Up Label
			var infoPickUpLabel = new UITextView(new RectangleF(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_TEXTVIEWS * 1.25f));
			infoPickUpLabel.Text = "Escribe en la parte de abajo los productos a traer o comprar.";
			infoPickUpLabel.Font = UIFont.FromName(LABEL_FONT, 12f);
			infoPickUpLabel.TextColor = UIColor.Gray;
			infoPickUpLabel.TextAlignment = UITextAlignment.Center;
			infoPickUpLabel.Editable = false;
			Add (infoPickUpLabel);

			//Items Text Field
			pYoffset += PADDING_BTWN_ELEMENT;
			itemsTextField = new UITextField(new RectangleF(MARGIN_TEXTFIELD, pYoffset, WIDTH-MARGIN_TEXTFIELD*2, HEIGHT_TEXTVIEWS));
			itemsTextField.Placeholder = "Agrega aquí el/los producto(s)";
			itemsTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsTextField.TextColor = UIColor.Gray;
			itemsTextField.TextAlignment = UITextAlignment.Center;
			itemsTextField.ClipsToBounds = true;
			itemsTextField.Layer.BorderColor = UIColor.DarkGray.CGColor;
			itemsTextField.Layer.BorderWidth = 0.0f;
			View.Add(itemsTextField);
			pYoffset += PADDING_BTWN_ELEMENT;

			var tapGesture = new UITapGestureRecognizer ((g) => {
				itemsTextField.ResignFirstResponder();
			});
			View.AddGestureRecognizer (tapGesture);

			//Toolbar with Done Button
			var toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			itemsTextField.InputAccessoryView = toolbar;
			toolbar.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					itemsTextField.ResignFirstResponder();
				})
			};
				

			//View Model Binding
			var set = this.CreateBindingSet<ItemsView, ItemsViewModel>();
			set.Bind(itemsTextField).To(vm => vm.Items);
			set.Apply();

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender,args) => {
				
					if (itemsTextField.Text.ToString ().Trim ().Equals ("")) {    

						new UIAlertView("¿Olvidaste, apeteces o necesitas algo?", "Favor de escribir sus productos"
							, null, "Ok", null).Show();
						 
					}else{

						var viewModel = (ItemsViewModel)ViewModel;
						viewModel.PublishData ();
						NavigationController.PopViewControllerAnimated(true);
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

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			itemsTextField.BecomeFirstResponder ();
		}
			
	}
}