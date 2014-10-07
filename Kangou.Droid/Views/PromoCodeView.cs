using Android.App;
using Android.OS;
using System;
using Cirrious.MvvmCross.Droid.Views;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{
	[Activity(Label = "Código de promoción", Icon="@drawable/icon")]
	public class PromoCodeView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.PromoCodeView);

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
				var viewModel = (PromoCodeViewModel)ViewModel;
				viewModel.PublishData ();
				Finish ();
				return true;
			default:
				return base.OnOptionsItemSelected(item);
			}
		}

    }
}