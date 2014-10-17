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


	[Activity(Label = "¿Dónde recolectar? (Opcional)")]
	public class PickUpView : MvxFragmentActivity
    {
	
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.PickUpView);

			/* Data Binding */
			var viewModel = (PickUpViewModel) ViewModel;
            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
			mapFragment.Map.MyLocationEnabled = true;

			/* Setting Geocoder and CameraChange listener */
			Geocoder geocoder = new Geocoder(Application.Context);
			IList<Address> addresses;
			System.Threading.Timer timer = null;
			mapFragment.Map.CameraChange += (object sender, GoogleMap.CameraChangeEventArgs e) => {

				if(timer != null){
					timer.Dispose();
					timer = null;
				}

				timer = new System.Threading.Timer ((o) => 
				{	
					try{
						addresses = geocoder.GetFromLocation( e.P0.Target.Latitude, e.P0.Target.Longitude, 1);
						if (addresses.Count == 0)
							return;
						Address address = addresses.First();
						String street = address.GetAddressLine(0);
						String city = address.GetAddressLine(1);
						viewModel.Street = street + ", " + city;
					}catch(Exception exception){
						System.Diagnostics.Debug.WriteLine("Exception: {0}",exception.ToString());
					};
				}, null, 50, 0);
			};

			/*
			string provider = LocationManager.GpsProvider;
			LocationManager locMgr = GetSystemService (Context.LocationService) as LocationManager;
			mapFragment.Map.MyLocationButtonClick += (object sender, GoogleMap.MyLocationButtonClickEventArgs e) => {
				if(locMgr.IsProviderEnabled(provider))
					updateToCameraToCurrentLocation(mapFragment.Map);
				else
					Toast.MakeText (this, "Favor de encender el GPS", ToastLength.Long).Show();			
			};
			*/

			updateToCameraToCurrentLocation(mapFragment.Map);
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
			
				var address = FindViewById<EditText> (Resource.Id.address);
				var references = FindViewById<EditText> (Resource.Id.references);

				if (references.Text.ToString ().Trim ().Equals ("")) {    
					references.SetError ("Favor de poner una referencia (ej. portón negro, negocio, etc.)", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					references.RequestFocus ();
					//Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				} 

				if (address.Text.ToString ().Trim ().Equals ("")) {    
					address.SetError ("¿Dónde vamos a comprar o recoger?", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					address.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				} 

				var viewModel = (PickUpViewModel)ViewModel;
				viewModel.PublishData ();
				Finish ();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

		private void updateToCameraToCurrentLocation(GoogleMap map){
			var lat = RegisterOrderViewModel.LOCATION.Lat;
			var lng = RegisterOrderViewModel.LOCATION.Lng;

			if (lat == 0)
				lat = 19.430566;

			if (lng == 0)
				lng = -99.200946;

			/* Setting initial configuration */
			LatLng location = new LatLng(lat, lng);
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(location);
			builder.Zoom(18);
			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
			map.AnimateCamera (cameraUpdate);
		}

    }
}