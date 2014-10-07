using Android.App;
using Android.OS;
using System;
using Cirrious.MvvmCross.Droid.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Android.Content;
using Android.Text;
using Kangou.Core.Helpers;
using Java.Lang;
using System.Text.RegularExpressions;
using Kangou.Core;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Threading;
using Android.Net;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{

	[Activity(Label = "Tarjeta de Crédito", Icon="@drawable/icon")]
	public class CreditCardView : MvxActivity
    {
		private BindableProgress _bindableProgress;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.CreditCardView);

			_bindableProgress = new BindableProgress(this);

			var set = this.CreateBindingSet<CreditCardView, CreditCardViewModel>();
			set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsBusy);
			set.Apply();

			var canEdit = true;

			var creditCardNumber = FindViewById<EditText> (Resource.Id.creditCardNumber);
			creditCardNumber.AfterTextChanged += (object sender, AfterTextChangedEventArgs e) => 
			{
				if(!canEdit)
					return;
				canEdit = false;
				{
					string creditCardNumberString = StringFormater.FormatCreditCardNumber(e.Editable.ToString());
					e.Editable.Clear();
					e.Editable.Append(creditCardNumberString);
				}
				canEdit = true;
			};

			var expirationDate = FindViewById<EditText> (Resource.Id.expirationDate);
			expirationDate.AfterTextChanged += (object sender, AfterTextChangedEventArgs e) => 
			{
				if(!canEdit)
					return;
				canEdit = false;
				{
					var expirationDateString = StringFormater.FormatExpirationDate(e.Editable.ToString());
					e.Editable.Clear();
					e.Editable.Append(expirationDateString);
				}
				canEdit = true;
			};
        }
	
		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.accept, menu);
			return base.OnPrepareOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.action_accept:

				var creditCardNumber = FindViewById<EditText> (Resource.Id.creditCardNumber);
				var expirationDate = FindViewById<EditText> (Resource.Id.expirationDate);
				var cvv = FindViewById<EditText> (Resource.Id.cvv);

				var creditCardString = creditCardNumber.Text.ToString ().Trim ();
				if (creditCardString.Equals ("") || !StringValidator.IsValidCreditCard (creditCardString)) {    
					creditCardNumber.SetError ("Se requiere un número de tarjeta válido", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					creditCardNumber.RequestFocus ();
					InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
					imm.ToggleSoftInput (Android.Views.InputMethods.ShowFlags.Forced, 0);
					return false;
				}

				var expirationDateString = expirationDate.Text.ToString ().Trim ();
				if (expirationDateString.Equals ("") || expirationDateString.Length < 4) {   
					expirationDate.SetError ("Fecha de expiración inválida", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					expirationDate.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				} 

				var cvvString = cvv.Text.ToString ().Trim ();
				if (cvvString.Equals ("") || cvvString.Length < 3) {    
					cvv.SetError ("CVV inválido", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					cvv.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				}

				var connectivityManager = (ConnectivityManager)GetSystemService (ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				if ((activeConnection != null) && activeConnection.IsConnected) {
				
					var viewModel = (CreditCardViewModel)ViewModel;
					viewModel.IsBusy = true;
					ThreadPool.QueueUserWorkItem (o => {

						var isSuccesful = viewModel.PublishData ();
	
						RunOnUiThread (() => {
							viewModel.IsBusy = false;

							if (isSuccesful)
								Finish ();
							else {
								var errorToRegisterDataDialog = new AlertDialog.Builder (this);
								errorToRegisterDataDialog.SetTitle ("Error al registrar datos");
								errorToRegisterDataDialog.SetMessage ("Favor de verificar que sus datos sean correctos o que tenga conexión a Internet");
								errorToRegisterDataDialog.SetPositiveButton ("Aceptar", (object sender, DialogClickEventArgs args)=>{});
								errorToRegisterDataDialog.Show();
							}

						});

					});

				} else {
					Toast.MakeText (this, "No se puede conectar a Internet", ToastLength.Long).Show();
				}

				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
			
    }
}