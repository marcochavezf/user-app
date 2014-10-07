using Android.App;
using Android.OS;
using System;
using Cirrious.MvvmCross.Droid.Views;
using Kangou.Core.Helpers;
using Android.Widget;
using Android.Views.InputMethods;
using Android.Content;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{
	[Activity(Label = "Orden confirmada", Icon="@drawable/icon")]
	public class ConfirmationOrderView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.ConfirmationOrderView);
        }

		public override void OnBackPressed ()
		{
			var viewModel = (ConfirmationOrderViewModel)ViewModel;
			viewModel.ResetOrderDataCommand.Execute (null);
			base.OnBackPressed ();
		}
    }
}