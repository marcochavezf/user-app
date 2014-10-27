using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using SlidingPanels.Lib.PanelContainers;
using Kangou.Core.ViewModels;
using Kangou.Core;
using MonoTouch.ObjCRuntime;

namespace Kangou.Touch
{
	public partial class EditProfileView : RootMvxViewController
    {
		public override void ViewDidLoad()
		{
			NavigationItem.Title = "Datos personales";

			//Constants
			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_WIDTH_SUBVIEWS = WIDTH / 8 * 0.5f; 
			var MARGIN_HEIGHT_SUBVIEWS = HEIGHT * 0.2f;
			var HEIGHT_TEXTFIELD = 60f;
			var LABEL_FONT_SIZE = 15f;
			var LABEL_FONT = "Arial-BoldMT";

			//Load Root View
			View = new UIView(){ BackgroundColor = UIColor.White};
			base.ViewDidLoad();
			var viewModel = (EditProfileViewModel)ViewModel;

			var pYoffset = MARGIN_HEIGHT_SUBVIEWS;

			//FullName
			var fullNameTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			fullNameTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			fullNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			fullNameTextField.Layer.BorderWidth = 0.5f;
			fullNameTextField.TextAlignment = UITextAlignment.Center;
			fullNameTextField.Placeholder = "Nombre";
			Add (fullNameTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//PhoneNumber
			var phoneNumberTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			phoneNumberTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			phoneNumberTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			phoneNumberTextField.Layer.BorderWidth = 0.5f;
			phoneNumberTextField.TextAlignment = UITextAlignment.Center;
			phoneNumberTextField.Placeholder = "Teléfono";
			phoneNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
			Add (phoneNumberTextField);
			pYoffset += HEIGHT_TEXTFIELD-0.5f;

			//Email
			var emailTextField = new UITextField(new RectangleF(MARGIN_WIDTH_SUBVIEWS, pYoffset, WIDTH-MARGIN_WIDTH_SUBVIEWS*2, HEIGHT_TEXTFIELD));
			emailTextField.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			emailTextField.Layer.BorderColor = UIColor.Gray.CGColor;
			emailTextField.Layer.BorderWidth = 0.5f;
			emailTextField.TextAlignment = UITextAlignment.Center;
			emailTextField.Placeholder = "Email";
			emailTextField.KeyboardType = UIKeyboardType.EmailAddress;
			emailTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
			Add (emailTextField);
			pYoffset += HEIGHT_TEXTFIELD-1;


			//Toolbar with Done Button for FullName
			var toolbarFullName = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarFullName.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					phoneNumberTextField.BecomeFirstResponder();
				})
			};
			fullNameTextField.InputAccessoryView = toolbarFullName;

			//Toolbar with Done Button for PhoneNumber
			var toolbarPhoneNumber = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarPhoneNumber.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					emailTextField.BecomeFirstResponder();
				})
			};
			phoneNumberTextField.InputAccessoryView = toolbarPhoneNumber;

			//Toolbar with Done Button for Email
			var toolbarEmail = new UIToolbar (new RectangleF (0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
			toolbarEmail.Items = new UIBarButtonItem[]{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					emailTextField.ResignFirstResponder();
				})
			};
			emailTextField.InputAccessoryView = toolbarEmail;

			//Binding
			var set = this.CreateBindingSet<EditProfileView, EditProfileViewModel>();
			set.Bind(fullNameTextField).To(vm => vm.Name);
			set.Bind(phoneNumberTextField).To(vm => vm.PhoneNumber);
			set.Bind(emailTextField).To(vm => vm.Email);
			set.Apply();


			var navigationController = NavigationController as SlidingPanelsNavigationViewController;

			if (String.IsNullOrWhiteSpace (viewModel.Name)) {
				NavigationItem.LeftBarButtonItem.Enabled = false;
				SlidingGestureRecogniser.EnableGesture = false;
			}

			//Add Button
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender,args) => {

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
								viewModel.SaveData();
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


    }
}

