using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging;
using Kangou.Core.Helpers;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using Android.Views.InputMethods;
using Android.Content;
using Android.Views;
using Android.Locations;
using System.Collections.Generic;
using System;
using System.Linq;
using Android.Telephony;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{

    [Activity(Label = "¿Dónde entregar?")]
    public class DropOffListView : MvxFragmentActivity
    {

		private const int DELETE_ITEM = 0;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.DropOffListView);

			var listView = FindViewById<ListView> (Resource.Id.listView);
			RegisterForContextMenu (listView);
		}

	
		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo info)
		{

			menu.Add (0,DELETE_ITEM,0,"Borrar");
		}

		public override bool OnContextItemSelected(IMenuItem item)
		{

			if (item.ItemId.Equals(DELETE_ITEM))
			{
				AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
							
				var listView = FindViewById<ListView> (Resource.Id.listView);
				var itemListView = listView.GetChildAt(info.Position);
				var textView = itemListView.FindViewById<TextView> (Resource.Id.id_dropoffdata);

				var idDropOffDataSelected = Convert.ToInt32(textView.Text);

				var viewModel = (DropOffListViewModel)ViewModel;
				viewModel.DeleteData(idDropOffDataSelected);
			}

			return base.OnOptionsItemSelected(item);
		}
	
			
    }
}