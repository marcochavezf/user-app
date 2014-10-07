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
	[Activity(Label = "¿Qué comprar o traer?", Icon="@drawable/icon")]
	public class ItemsView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.ItemsView);

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

				InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);

				var itemsEditText = FindViewById<EditText> (Resource.Id.items);
				if (itemsEditText.Text.ToString ().Trim ().Equals ("")) {    
					itemsEditText.SetError ("¿Olvidaste, apeteces o necesitas algo?", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					itemsEditText.RequestFocus ();

					imm.ToggleSoftInput (Android.Views.InputMethods.ShowFlags.Forced, 0);
					return false;
				}
				imm.ToggleSoftInput (Android.Views.InputMethods.ShowFlags.Forced, 0);

				var viewModel = (ItemsViewModel)ViewModel;
				viewModel.PublishData ();

				Finish ();
				return true;
			default:
				return base.OnOptionsItemSelected(item);
			}
		}
    }
}