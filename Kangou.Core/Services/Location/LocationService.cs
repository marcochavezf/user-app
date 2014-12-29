/*
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Helpers;
using Kangou.Core.ViewModels;
using System.Diagnostics;

namespace Kangou.Core.Services.Location
{
    public class LocationService
        : ILocationService
    {
        private readonly IMvxLocationWatcher _watcher;

        public LocationService(IMvxLocationWatcher watcher)
        {
            _watcher = watcher;
            _watcher.Start(new MvxLocationOptions(), OnLocation, OnError);
        }

        private void OnLocation(MvxGeoLocation location)
        {
			Debug.WriteLine (location.Coordinates);
			RegisterOrderViewModel.LOCATION.Lat = location.Coordinates.Latitude;
			RegisterOrderViewModel.LOCATION.Lng = location.Coordinates.Longitude;
			//_watcher.Stop ();
        }

        private void OnError(MvxLocationError error)
        {
            Mvx.Error("Seen location error {0}", error.Code);
        }

    }
}*/