using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using SlidingPanels.Lib.PanelContainers;
using Kangou.Core.ViewModels;
using Kangou.Core;
using ObjCRuntime;

namespace Kangou.Touch
{
	public partial class EditProfileView : RootMvxViewController
    {

		EditProfileViewModel _viewModel;

		public override void ViewDidLoad()
		{
			NavigationItem.Title = "Datos personales";

			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH * 0.1f; 
			var HEIGHT_TEXTFIELD = HEIGHT * 0.1f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			_viewModel = (EditProfileViewModel)ViewModel;
			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_PRIMARY;

			var pYoffset = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height + MARGIN_WIDTH_SUBVIEWS;

			//FullName
			var fullNameTextField = new UITextField(new CGRect(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			fullNameTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			fullNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			fullNameTextField.Layer.BorderWidth = 0.5f;
			fullNameTextField.TextAlignment = UITextAlignment.Center;
			fullNameTextField.Placeholder = "Nombre";
			fullNameTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (fullNameTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//PhoneNumber
			var phoneNumberTextField = new UITextField(new CGRect(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			phoneNumberTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			phoneNumberTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			phoneNumberTextField.Layer.BorderWidth = 0.5f;
			phoneNumberTextField.TextAlignment = UITextAlignment.Center;
			phoneNumberTextField.Placeholder = "Teléfono";
			phoneNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
			phoneNumberTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (phoneNumberTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//Email
			var emailTextField = new UITextField(new CGRect(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			emailTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			emailTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			emailTextField.Layer.BorderWidth = 0.5f;
			emailTextField.TextAlignment = UITextAlignment.Center;
			emailTextField.Placeholder = "Email";
			emailTextField.KeyboardType = UIKeyboardType.EmailAddress;
			emailTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			emailTextField.BackgroundColor = Constants.LABEL_BACKGROUND_COLOR;
			Add (emailTextField);
			pYoffset += HEIGHT_TEXTFIELD-1;

			var tapGesture = new UITapGestureRecognizer ((g) => {
				fullNameTextField.ResignFirstResponder();
				phoneNumberTextField.ResignFirstResponder();
				emailTextField.ResignFirstResponder();
			});
			View.AddGestureRecognizer (tapGesture);


			//Binding
			var set = this.CreateBindingSet<EditProfileView, EditProfileViewModel>();
			set.Bind(fullNameTextField).To(vm => vm.Name);
			set.Bind(phoneNumberTextField).To(vm => vm.PhoneNumber);
			set.Bind(emailTextField).To(vm => vm.Email);
			set.Apply();


			var navigationController = NavigationController as SlidingPanelsNavigationViewController;

			if (String.IsNullOrWhiteSpace (_viewModel.Name)) {
				NavigationItem.LeftBarButtonItem.Enabled = false;
				SlidingGestureRecogniser.EnableGesture = false;
			}

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem("Guardar", UIBarButtonItemStyle.Done, (sender,args) => {

					var fullNameString = fullNameTextField.Text.Trim ();
					var phoneNumberString = phoneNumberTextField.Text.Trim ();
					var emailString = emailTextField.Text.Trim ();

					//Check if data is well formatted, otherwise publish data to ViewModel
					if (fullNameString.Equals("")){
						var alert = new UIAlertView("¿Cuál es tu nombre?", ""
							, null, "Ok", null);
						alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
							if(e.ButtonIndex == 0)
								fullNameTextField.BecomeFirstResponder();
						};
						alert.Show();
					}else

						if (phoneNumberString.Equals ("") || phoneNumberString.Length < 8){
							var alert = new UIAlertView("Favor de escribir un número celular válido", ""
								, null, "Ok", null);
							alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
								if(e.ButtonIndex == 0)
									phoneNumberTextField.BecomeFirstResponder();
							};
							alert.Show();
						}else

							if (emailString.Equals("")|| !StringValidator.IsValidEmail(emailString)){
								var alert = new UIAlertView("Ingresa un correo válido", ""
									, null, "Ok", null);
								alert.Clicked += (object alertSender, UIButtonEventArgs e) => {
									if(e.ButtonIndex == 0)
										emailTextField.BecomeFirstResponder();
								};
								alert.Show();
							} else {
								_viewModel.SaveData();
								var alert = new UIAlertView("Tu información se ha guardado", ""
									, null, "Ok", null);
								alert.Clicked += delegate {
									navigationController.TogglePanel(PanelType.LeftPanel);
									NavigationItem.LeftBarButtonItem.Enabled = true;
									SlidingGestureRecogniser.EnableGesture = true;
								};
								alert.Show();
							}
				})
				, true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_viewModel.PublishMessageViewOpened ();
		}
    }
}

