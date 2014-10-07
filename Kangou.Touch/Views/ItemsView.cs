using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;

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

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH / 8; 

			var PADDING_BTWN_ELEMENT = 90;
			var PADDING_SECTION = 100;
			var HEIGHT_BUTTON = 50;
			var HEIGHT_TEXTVIEWS = 80;

			var LABEL_FONT_SIZE = 15f;
			var BUTTON_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";
			var BUTTON_FONT = "Arial-BoldMT";

			var pY = 40f;

			//Items Text Field
			itemsTextField = new UITextView(new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_TEXTVIEWS));
			itemsTextField.Text = "Agrega aquí los productos a comprar o traer";
			itemsTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsTextField.TextColor = UIColor.Gray;
			itemsTextField.TextAlignment = UITextAlignment.Left;
			itemsTextField.Layer.BorderColor = UIColor.DarkGray.CGColor;
			itemsTextField.Layer.BorderWidth = 0.5f;
			itemsTextField.Editable = true;
			Add(itemsTextField);
			pY += PADDING_BTWN_ELEMENT;

			//Toolbar with Done Button
			var toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			itemsTextField.InputAccessoryView = toolbar;
			toolbar.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					itemsTextField.ResignFirstResponder();
				})
			};

			//Info Pick Up Label
			var infoPickUpLabel = new UITextView(new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_TEXTVIEWS*1.5f));
			infoPickUpLabel.Text = "*Buscaremos lo que necesites, no te preocupes por la dirección.\n\nEn caso de ser un lugar específico, indícanos a donde ir:";
			infoPickUpLabel.Font = UIFont.FromName(LABEL_FONT, 12f);
			infoPickUpLabel.TextColor = UIColor.Gray;
			infoPickUpLabel.TextAlignment = UITextAlignment.Justified;
			infoPickUpLabel.Editable = false;
			var tapGesture = new UITapGestureRecognizer ((g) => {
				itemsTextField.ResignFirstResponder();
			});
			infoPickUpLabel.AddGestureRecognizer (tapGesture);
			Add(infoPickUpLabel);
			pY += PADDING_SECTION;

			//Add Pick Up Address Button
			var addPickUpAddressButton = new UIButton (UIButtonType.RoundedRect);
			addPickUpAddressButton.SetTitle ("Agregar dirección (Opcional)", UIControlState.Normal);
			addPickUpAddressButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			addPickUpAddressButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			addPickUpAddressButton.Layer.BorderColor = UIColor.Gray.CGColor;
			addPickUpAddressButton.Layer.BorderWidth = 0.5f;
			Add (addPickUpAddressButton);

			//View Model Binding
			var set = this.CreateBindingSet<ItemsView, ItemsViewModel>();
			set.Bind(itemsTextField).To(vm => vm.Items);
			set.Bind(addPickUpAddressButton).To(vm => vm.AddPickUpAddressCommand);
			set.Bind(addPickUpAddressButton).For("Title").To(vm => vm.PickUpAddress);
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

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			itemsTextField.BecomeFirstResponder ();
		}
			
	}
}