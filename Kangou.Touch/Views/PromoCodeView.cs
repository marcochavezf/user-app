using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;

namespace Kangou.Touch.Views
{
	[Register("PromoCodeView")]
	public class PromoCodeView : MvxViewController
	{
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

			var PADDING_LABEL_BUTTON = 25;
			var PADDING_SECTION = 80;
			var HEIGHT_BUTTON = 50;

			var LABEL_FONT_SIZE = 15f;
			var BUTTON_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";
			var BUTTON_FONT = "Arial-BoldMT";

			var pY = 40f;

			//Items
			var itemsLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			itemsLabel.Text = "1. ¿Qué comprar o traer?";
			itemsLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			itemsLabel.TextColor = UIColor.Gray;
			itemsLabel.TextAlignment = UITextAlignment.Center;
			Add(itemsLabel);
			pY += PADDING_LABEL_BUTTON;

			var itemsButton = new UIButton (UIButtonType.RoundedRect);
			itemsButton.SetTitle ("Agregar productos", UIControlState.Normal);
			itemsButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			itemsButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			itemsButton.Layer.BorderColor = UIColor.Gray.CGColor;
			itemsButton.Layer.BorderWidth = 1.0f;
			itemsButton.Layer.CornerRadius = 0.5f;
			Add (itemsButton);

			pY += PADDING_SECTION;

			//Drop Off
			var dropOffLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			dropOffLabel.Text = "2. ¿Dónde entregar?";
			dropOffLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			dropOffLabel.TextColor = UIColor.Gray;
			dropOffLabel.TextAlignment = UITextAlignment.Center;
			Add(dropOffLabel);
			pY += PADDING_LABEL_BUTTON;

			var dropOffButton = new UIButton (UIButtonType.RoundedRect);
			dropOffButton.SetTitle ("Agregar dirección", UIControlState.Normal);
			dropOffButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			dropOffButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			dropOffButton.Layer.BorderColor = UIColor.Gray.CGColor;
			dropOffButton.Layer.BorderWidth = 1.0f;
			dropOffButton.Layer.CornerRadius = 0.5f;
			Add (dropOffButton);

			pY += PADDING_SECTION;

			//Payment Method
			var paymentMethodLabel = new UILabel(new RectangleF(MARGIN_SUBVIEWS-7.5f, pY, WIDTH-MARGIN_SUBVIEWS*2, 20));
			paymentMethodLabel.Text = "3. Información de pago";
			paymentMethodLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			paymentMethodLabel.TextColor = UIColor.Gray;
			paymentMethodLabel.TextAlignment = UITextAlignment.Center;
			Add(paymentMethodLabel);
			pY += PADDING_LABEL_BUTTON;

			var paymentMethodButton = new UIButton (UIButtonType.RoundedRect);
			paymentMethodButton.SetTitle ("Seleccionar forma de pago", UIControlState.Normal);
			paymentMethodButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			paymentMethodButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			paymentMethodButton.Layer.BorderColor = UIColor.Gray.CGColor;
			paymentMethodButton.Layer.BorderWidth = 1.0f;
			paymentMethodButton.Layer.CornerRadius = 0.5f;
			Add (paymentMethodButton);

			pY += PADDING_SECTION * 1.75f;

			var pedirUnKangouButton = new UIButton (UIButtonType.RoundedRect);
			pedirUnKangouButton.SetTitle ("Pedir un Kangou", UIControlState.Normal);
			pedirUnKangouButton.Font = UIFont.FromName(BUTTON_FONT, BUTTON_FONT_SIZE);
			pedirUnKangouButton.Frame = new RectangleF(MARGIN_SUBVIEWS, pY, WIDTH-MARGIN_SUBVIEWS*2, HEIGHT_BUTTON);
			pedirUnKangouButton.Layer.BorderColor = UIColor.Gray.CGColor;
			pedirUnKangouButton.Layer.BorderWidth = 1.0f;
			pedirUnKangouButton.Layer.CornerRadius = 0.5f;
			Add (pedirUnKangouButton);

			var set = this.CreateBindingSet<PromoCodeView, PromoCodeViewModel>();
			//set.Bind(itemsButton).To(vm => vm.AddItemsCommand);
			set.Apply();
		}
	}
}