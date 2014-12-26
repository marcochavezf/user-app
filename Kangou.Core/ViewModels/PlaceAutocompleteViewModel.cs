using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using System.Collections.Generic;
using Kangou.Core.ViewModels.ObserverMessages;
using System;
using System.Threading.Tasks;
using Kangou.Core.ViewModels;
using Cirrious.CrossCore.Platform;
using Kangou.Core.WebClients;

namespace Kangou.Core.ViewModels
{
	public class PlaceAutocompleteViewModel
		: MvxViewModel
    {
		private KangouClient _kangouClient;
		private IMvxMessenger _messenger;


		public PlaceAutocompleteViewModel (IDataService dataService, IMvxMessenger messenger, IMvxJsonConverter _jsonConverter)
		{
			_kangouClient = new KangouClient (_jsonConverter);
			_messenger = messenger;
		}

		public void PopulateListFromServer ()
		{
			_kangouClient.GetPlacesList (InputToPredict, 
				(placesList) => 
				{
					InvokeOnMainThread (delegate {
						PlacesList = placesList;
					});
				},
				(error) => 
				{
					PlacesList = new List<Prediction> ();
					Debug.WriteLine ("error: {0}", error);
				});
		}

		private bool _isTableVisible;
		public bool IsTableVisible
		{
			get { return _isTableVisible; }
			set 
			{
				_isTableVisible = value; 
				RaisePropertyChanged(() => IsTableVisible); 
			}
		}

		private string _inputToPredict;
		public string InputToPredict { 
			get { return _inputToPredict; }
			set 
			{
				_inputToPredict = value;
				RaisePropertyChanged (() => InputToPredict);
				PopulateListFromServer (); 
			}
		}

		private List<Prediction> _placesList;
		public List<Prediction> PlacesList
		{
			get { return _placesList; }
			set 
			{ 
				_placesList = value; 
				RaisePropertyChanged(() => PlacesList);
				IsTableVisible = PlacesList.Count > 0;
			}
		}

		public ICommand SelectPlaceCommand
		{
			get
			{
				return new MvxCommand<Prediction>(place => {
					_kangouClient.GetLatLngFromPlaceId (place.place_id, 
						(placeDetailsResponse) => 
						{
							_messenger.Publish (new PlaceDetailsResponseMessage (this, placeDetailsResponse));
							InvokeOnMainThread(delegate {
								Close(this);
							});
						},
						(error) => 
						{
							Debug.WriteLine ("error: {0}", error);
						});
				});
			}
		}
	}
}