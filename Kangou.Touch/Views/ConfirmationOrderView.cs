using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using Kangou.Core.ViewModels;
using Kangou.Core.Helpers;

namespace Kangou.Touch.Views
{
	[Register("ConfirmationOrderView")]
	public class ConfirmationOrderView : MvxViewController
	{
		public override void ViewDidLoad()
		{
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();

			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH / 8; 
			var MARGIN_HEIGHT_SUBVIEWS = MARGIN_WIDTH_SUBVIEWS * 0.5f;
			var HEIGHT_BUTTON = 40f;
			var HEIGHT_TEXTLABEL = 110f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Kangou Image
			var kangouImage = new UIImageView (UIImage.FromBundle("big_icon.png"));
			var widthImage = 260f;
			var heightImage = 260f;
			var marginWidthImage = WIDTH * 0.5f - widthImage * 0.5f;
			var pYoffset = marginWidthImage * 0.5f;
			kangouImage.Frame = new CGRect (marginWidthImage, pYoffset, widthImage, heightImage);
			Add (kangouImage);
			pYoffset += heightImage + MARGIN_HEIGHT_SUBVIEWS;

			//Confirmation Message Text View
			var confirmationMessageTextView = new UITextView(new CGRect(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTLABEL));
			confirmationMessageTextView.Text = StringMessages.ORDER_CONFIRMED_MESSAGE;
			confirmationMessageTextView.Font = UIFont.FromName(LABEL_FONT, 12f);
			confirmationMessageTextView.TextColor = UIColor.Gray;
			confirmationMessageTextView.TextAlignment = UITextAlignment.Center;
			confirmationMessageTextView.Editable = false;
			Add(confirmationMessageTextView);


			//Come back Button
			pYoffset = HEIGHT - HEIGHT_BUTTON  - MARGIN_WIDTH_SUBVIEWS * 2.5f;
			var comeBackButton = new UIButton (UIButtonType.RoundedRect);
			comeBackButton.SetTitle ("Regresar", UIControlState.Normal);
			comeBackButton.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			comeBackButton.Frame = new CGRect(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_BUTTON);
			comeBackButton.Layer.BorderColor = UIColor.Gray.CGColor;
			comeBackButton.Layer.BorderWidth = 0.5f;
			Add (comeBackButton);

			//Binding
			var set = this.CreateBindingSet<ConfirmationOrderView, ConfirmationOrderViewModel>();
			set.Bind(comeBackButton).To(vm => vm.ResetOrderDataCommand);
			set.Apply();
		}

		public override void ViewWillDisappear (bool animated)
		{
			var viewModel = (ConfirmationOrderViewModel)ViewModel;
			viewModel.ResetOrderDataCommand.Execute (null);
			base.ViewWillDisappear (animated);
		}
	}
}