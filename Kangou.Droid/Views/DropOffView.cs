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
using Kangou.Core;
using Kangou.Core.ViewModels;

namespace Kangou.Droid.Views
{

    [Activity(Label = "¿Dónde entregar?")]
    public class DropOffView : MvxFragmentActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.DropOffView);

			/* Data Binding */
			var viewModel = (DropOffViewModel) ViewModel;
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
						viewModel.Address = street + ", " + city;
					}catch(Exception exception){
						System.Diagnostics.Debug.WriteLine("Exception: {0}",exception.ToString());
					};

				}, null, 50, 0);
			};

			var lat = RegisterOrderViewModel.LOCATION.Lat;
			var lng = RegisterOrderViewModel.LOCATION.Lng;

			if (lat == 0)
				lat = 19.4121003;

			if (lng == 0)
				lng = -99.1805528;

			/* Setting initial configuration */
			LatLng location = new LatLng(lat,lng);
			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(location);
			builder.Zoom(14);
			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
			mapFragment.Map.MoveCamera(cameraUpdate);

			/* Add full phone format */
			EditText phoneNumber = FindViewById<EditText> (Resource.Id.phoneNumber);
			phoneNumber.AddTextChangedListener(new PhoneNumberFormattingTextWatcher());
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

				var fullName = FindViewById<EditText> (Resource.Id.fullName);
				var phoneNumber = FindViewById<EditText> (Resource.Id.phoneNumber);
				var address = FindViewById<EditText> (Resource.Id.address);
				var email = FindViewById<EditText> (Resource.Id.email);
				var references = FindViewById<EditText> (Resource.Id.references);

				if (fullName.Text.ToString ().Trim ().Equals ("")) {    
					fullName.SetError ("¿A quién vamos a entregar?", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					fullName.RequestFocus ();
					InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
					imm.ToggleSoftInput (Android.Views.InputMethods.ShowFlags.Forced, 0);
					return false;
				}

				var phoneNumberString = phoneNumber.Text.ToString ().Trim ();
				if (phoneNumberString.Equals ("") || phoneNumberString.Length < 8) {   
					phoneNumber.SetError ("Para comunicación rápida si surge algo imprevisto", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					phoneNumber.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				} 

				var emailAddress = email.Text.ToString ().Trim ();
				if (emailAddress.Equals ("") || !StringValidator.IsValidEmail(emailAddress)) {    
					email.SetError ("Ingresa un correo válido para enviarte la confirmación de la orden", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					email.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				}

				if (address.Text.ToString ().Trim ().Equals ("")) {    
					address.SetError ("¿Dónde vamos a entregar?", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					address.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				}

				if (references.Text.ToString ().Trim ().Equals ("")) {    
					references.SetError ("Favor de poner una referencia (ej. portón negro, negocio, etc.)", Resources.GetDrawable (Resource.Drawable.ic_action_accept));
					references.RequestFocus ();
					Window.SetSoftInputMode (Android.Views.SoftInput.StateAlwaysVisible);
					return false;
				} 

				var viewModel = (DropOffViewModel)ViewModel;
				viewModel.PublishData ();
				Finish ();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
    }
}