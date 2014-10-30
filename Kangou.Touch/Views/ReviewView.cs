using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Kangou.Core;
using Kangou.Touch.Views;
using SlidingPanels.Lib;

namespace Kangou.Touch
{
	[Register("ReviewView")]
	public class ReviewView : BusyMvxViewController
    {
	

        public override void ViewDidLoad()
        {
			_popNextToLastViewController = true; 
			_hideBackButton = true;
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
			var posYoffset = posYinst + posXtitle * 0.5f;


			//Commnents
			var titleCommentsLabel = new UILabel(new RectangleF(posXtitle, posYoffset, widthLabel, heightLabel));
			titleCommentsLabel.Text = "Comentarios acerca del cliente";
			titleCommentsLabel.TextAlignment = UITextAlignment.Center;
			titleCommentsLabel.Font = UIFont.FromName(Constants.LABEL_BOLD_FONT, Constants.LABEL_FONT_SIZE);
			titleCommentsLabel.TextColor = UIColor.Gray;
			Add(titleCommentsLabel);

			posYoffset += 50f;
			var widthTextField = WIDTH * 0.8f;
			var heightTextField = HEIGHT * 0.125f;
			var posXtextField = (WIDTH - widthTextField) * 0.5f;
			var commentsAboutClientTextField = new UITextView(new RectangleF(posXtextField, posYoffset, widthTextField, heightTextField));
			commentsAboutClientTextField.Font =  UIFont.FromName(Constants.LABEL_BOLD_FONT, Constants.LABEL_FONT_SIZE);
			commentsAboutClientTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			commentsAboutClientTextField.Layer.BorderWidth = 0.5f;
			commentsAboutClientTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add(commentsAboutClientTextField);

			//Rating
			posYoffset += 80f;
			var titleRatingLabel = new UILabel(new RectangleF(posXtitle, posYoffset, widthLabel, heightLabel));
			titleRatingLabel.Text = "Calificación acerca del cliente";
			titleRatingLabel.TextAlignment = UITextAlignment.Center;
			titleRatingLabel.Font = UIFont.FromName(Constants.LABEL_BOLD_FONT, Constants.LABEL_FONT_SIZE);
			titleRatingLabel.TextColor = UIColor.Gray;
			Add(titleRatingLabel);

			posYoffset += 50f;
			AddReviewStars (WIDTH, posYoffset, viewModel);

			View.AddGestureRecognizer (new UITapGestureRecognizer ((g) => {
				commentsAboutClientTextField.ResignFirstResponder();
			}));

			var errorHasntReviewedAlert = new UIAlertView ("Favor de calificar el servicio", "", null, "Ok");

			//Add Button
			var rightButton = new UIBarButtonItem ("Aceptar", UIBarButtonItemStyle.Done, (sender, args) => {
				if(viewModel.RatingAboutClient > 0){

					var reviewedFinishedAlert = new UIAlertView ("Orden finalizada", "\n¡Muchas gracias por usar nuestro servicio! ", null, "Ok");
					reviewedFinishedAlert.Clicked += (object alertSender, UIButtonEventArgs eventArgsAlert) => {
						viewModel.AcceptCommand.Execute(null);
					};
					reviewedFinishedAlert.Show ();

				} else { 
					errorHasntReviewedAlert.Show();
				}
			});
			NavigationItem.SetRightBarButtonItem(rightButton, true);

			//Binding
			var set = this.CreateBindingSet<ReviewView, ReviewViewModel>();
			set.Bind(commentsAboutClientTextField).To(vm => vm.CommentsAboutClient);	
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
            set.Apply();

			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

        }

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			SlidingGestureRecogniser.EnableGesture = false;
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			SlidingGestureRecogniser.EnableGesture = true;
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