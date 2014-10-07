using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Android.Widget;
using Android.Content;
using Kangou.Core.Helpers;
using Android.Net;
using Java.Lang;
using Android.Graphics;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{
	[Activity(Label = "Registra"+" tu"+" Orden")]
    public class RegisterOrderView : MvxActivity
    {
		private const int CREDIT_CARD_ID = 0;
		private const int CASH_ID = 1;
		RegisterOrderViewModel _viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.RegisterOrderView);
			_viewModel = (RegisterOrderViewModel)ViewModel;

			var paymentMethodDialogBuilder = new AlertDialog.Builder(this);
			paymentMethodDialogBuilder.SetTitle("Selecciona "+"una"+" forma"+" de"+" pago");
			paymentMethodDialogBuilder.SetItems(Resource.Array.list_dialog_items, PaymentMethodListClicked);
			var paymentMethodButton = FindViewById<Button>(Resource.Id.paymentMethodButton);
			paymentMethodButton.Click += delegate
			{
				if(_viewModel.IsDeliveryDataSet())
					paymentMethodDialogBuilder.Show();
			};

			var confirmOrderDialogBuilder = new AlertDialog.Builder(this);
			confirmOrderDialogBuilder.SetTitle("Confirmación de orden");
			confirmOrderDialogBuilder.SetMessage(Resource.String.confirm_message);
			confirmOrderDialogBuilder.SetNegativeButton("Cancelar",(object sender, DialogClickEventArgs args)=>{});
			confirmOrderDialogBuilder.SetPositiveButton("Confirmar",ConfirmOrderClicked);
			var pideUnKangouButton = FindViewById<Button>(Resource.Id.pideUnKangou);
			pideUnKangouButton.Click += delegate
			{
				if(_viewModel.IsDeliveryDataSet())
				{
					if (_viewModel.IsPaymentInfoSet())
						confirmOrderDialogBuilder.Show();
					else
						paymentMethodDialogBuilder.Show();
				}
			};			

			_viewModel.DisableButtons = () => {

				FindViewById<Button> (Resource.Id.number2).SetTextColor (Color.Gray);
				FindViewById<Button> (Resource.Id.addDropOffAddress).SetTextColor (Color.Gray);
				FindViewById<Button> (Resource.Id.number3).SetTextColor (Color.Gray);
				FindViewById<Button> (Resource.Id.paymentMethodButton).SetTextColor (Color.Gray);
				FindViewById<Button> (Resource.Id.pideUnKangou).Visibility = Android.Views.ViewStates.Invisible;
			};
			_viewModel.DisableButtons ();

			var color = Color.Rgb (255, 150, 11);

			_viewModel.EnableDropOffButton = ()=>{
				FindViewById<Button> (Resource.Id.number2).SetTextColor (color);
				FindViewById<Button> (Resource.Id.addDropOffAddress).SetTextColor (color);
			};

			_viewModel.EnablePaymentMethodButton = ()=>{
				_viewModel.EnableDropOffButton();
				FindViewById<Button> (Resource.Id.number3).SetTextColor (color);
				FindViewById<Button> (Resource.Id.paymentMethodButton).SetTextColor (color);
			};

			_viewModel.EnablePUKButton = ()=>{
				_viewModel.EnablePaymentMethodButton ();
				RunOnUiThread(()=>{
					FindViewById<Button> (Resource.Id.pideUnKangou).Visibility = Android.Views.ViewStates.Visible;
				});
			};
        }

		private void ConfirmOrderClicked(object sender, DialogClickEventArgs args)
		{
			var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
			var activeConnection = connectivityManager.ActiveNetworkInfo;
			if ((activeConnection != null) && activeConnection.IsConnected) {

				ProgressDialog dialog = ProgressDialog.Show (this, "Enviando datos", "Espere por favor...", true);

				var errorOrderResponseDialog = new AlertDialog.Builder (this);
				errorOrderResponseDialog.SetTitle ("Error al procesar la orden");
				errorOrderResponseDialog.SetMessage ("Compruebe su conexión a Internet e intente de nuevo.");
				errorOrderResponseDialog.SetNeutralButton ("Aceptar", (object senderErrorDialog, DialogClickEventArgs argsErrorDialog) => {
				});

				try
				{
					_viewModel.ConfirmOrder (
						(sucessResponse) => {
							dialog.Dismiss ();
						},
						(errorResponse) => {
							dialog.Dismiss ();
							RunOnUiThread(()=>{errorOrderResponseDialog.Show ();});
						}
					);

				}catch (Exception e){
					errorOrderResponseDialog.Show ();
				}

			} else {
				Toast.MakeText (this, "No se puede conectar a Internet", ToastLength.Long).Show();
			}

		}

		private void PaymentMethodListClicked(object sender, DialogClickEventArgs args)
		{
			switch (args.Which) {

			case CREDIT_CARD_ID:
				_viewModel.AddCreditCardCommand.Execute (null);
				break;

			case CASH_ID:
				_viewModel.SetCashPaymentMethod();
				break;

			default:
				_viewModel.InfoPaymentMethod = "Seleccionar forma de pago";
				break;

			}
		}
    }
}