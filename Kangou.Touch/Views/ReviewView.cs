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
using System.Collections.Generic;
using Kangou.Core;
using Kangou.Touch.Views;

namespace Kangou.Touch
{
	[Register("ReviewView")]
	public class ReviewView : BusyMvxViewController
    {
	

        public override void ViewDidLoad()
        {
			_popNextToLastViewController = true; 
			base.ViewDidLoad ();
			var viewModel = (ReviewViewModel)ViewModel;

			NavigationItem.Title = "Calificación del servicio";

			//Constants
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;

			var widthLabel = WIDTH * 0.8f;
			var heightLabel = Constants.HEIGHT_BUTTON;
			var posXtitle  = (WIDTH - widthLabel) * 0.5f;
			var posYinst = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height;
			var posYoffset = posYinst + posXtitle;

			//Rating
			var titleRatingLabel = new UILabel(new RectangleF(posXtitle, posYoffset, widthLabel, heightLabel));
			titleRatingLabel.Text = "Calificación acerca del cliente";
			titleRatingLabel.TextAlignment = UITextAlignment.Center;
			Add(titleRatingLabel);

			posYoffset += 40f;
			AddReviewStars (WIDTH, posYoffset, viewModel);

			//Commnents
			posYoffset += 70f;
			var titleCommentsLabel = new UILabel(new RectangleF(posXtitle, posYoffset, widthLabel, heightLabel));
			titleCommentsLabel.Text = "Comentarios acerca del cliente";
			titleCommentsLabel.TextAlignment = UITextAlignment.Center;
			Add(titleCommentsLabel);

			posYoffset += 50f;
			var widthTextField = WIDTH * 0.8f;
			var heightTextField = HEIGHT * 0.125f;
			var posXtextField = (WIDTH - widthTextField) * 0.5f;
			var commentsAboutClientTextField = new UITextView(new RectangleF(posXtextField, posYoffset, widthTextField, heightTextField));
			commentsAboutClientTextField.Font =  UIFont.FromName(Constants.LABEL_NORMAL_FONT, Constants.LABEL_FONT_SIZE);
			commentsAboutClientTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			commentsAboutClientTextField.Layer.BorderWidth = 0.5f;
            Add(commentsAboutClientTextField);

			/*
			if (!UIDevice.CurrentDevice.CheckSystemVersion (7, 8)) {
				//Version 7 Toolbar with Done Button
				Console.WriteLine ("Version 7");
				var toolbar = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
				commentsAboutClientTextField.InputAccessoryView = toolbar;
				toolbar.Items = new UIBarButtonItem[] {
					new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
					new UIBarButtonItem (UIBarButtonSystemItem.Done, delegate {
						commentsAboutClientTextField.ResignFirstResponder ();
					})
				};
			}
			*/

			var errorHasntReviewedAlert = new UIAlertView ("Favor de calificar el servicio", "", null, "Ok");

			//Accept Button
			var widthButton = WIDTH * Constants.PROPORTION_BUTTON;
			var posXbutton = (WIDTH - widthButton) * 0.5f;
			var posYbutton = HEIGHT - Constants.HEIGHT_BUTTON * 2.5f;
			var acceptButton = new UIButton (UIButtonType.RoundedRect);
			acceptButton.SetTitle ("Aceptar", UIControlState.Normal);
			acceptButton.Frame = new RectangleF (posXbutton, posYbutton, widthButton, Constants.HEIGHT_BUTTON);
			acceptButton.Layer.BorderColor = UIColor.Gray.CGColor;
			acceptButton.Layer.BorderWidth = 0.5f;
			acceptButton.TouchUpInside += (object sender, EventArgs e) => {
				if(viewModel.RatingAboutClient > 0){

					var reviewedFinishedAlert = new UIAlertView ("Orden finalizada", "\n¡Muchas gracias por usar nuestro servicio! ", null, "Ok");
					reviewedFinishedAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
						viewModel.AcceptCommand.Execute(null);
					};
					reviewedFinishedAlert.Show ();

				} else { 
					errorHasntReviewedAlert.Show();
				}
			};
			Add (acceptButton);

			//Binding
			var set = this.CreateBindingSet<ReviewView, ReviewViewModel>();
			set.Bind(commentsAboutClientTextField).To(vm => vm.CommentsAboutClient);	
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
            set.Apply();
        }

		private void AddReviewStars (float WIDTH, float pY, ReviewViewModel viewModel)
		{
			var starButtons = new List<UIButton> ();

			var totalNumStars = 5;
			var heightImage = 50f;
			var widthImage = 50f;
			var marginWidth = widthImage * 0.15f;
			var pXstars = (WIDTH - (widthImage * totalNumStars + marginWidth * (totalNumStars - 1))) * .5f;
			var pYstars = pY;
			for (int i = 0; i < totalNumStars; i++) {
				var starIndex = i;
				var starReviewButton = new UIButton (UIButtonType.RoundedRect);
				var pXOffset = pXstars + (widthImage + marginWidth) * i;
				starReviewButton.SetBackgroundImage( UIImage.FromBundle ("starReviewOff.png"), UIControlState.Normal);
				starReviewButton.Frame = new RectangleF (pXOffset, pYstars, widthImage, heightImage);
				starReviewButton.TouchUpInside += (object sender, EventArgs e) => {
					viewModel.RatingAboutClient = starIndex + 1;
					for(int j=0; j<starButtons.Count; j++){
						var imagePath = "starReviewOff.png";
						if (j <= starIndex)
							imagePath = "starReviewOn.png";
						starButtons[j].SetBackgroundImage( UIImage.FromBundle (imagePath), UIControlState.Normal);
					}
				};
				starButtons.Add (starReviewButton);
				Add (starReviewButton);
			}
		}
    }
}