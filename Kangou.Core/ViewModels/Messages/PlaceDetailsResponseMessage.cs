using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;
using Kangou.Core.ViewModels;
using Kangou.Core.Helpers;

namespace Kangou.Core
{

	/*************************
	 * Location Data Message
	 * */
	public class PlaceDetailsResponseMessage 
		: MvxMessage
	{
		public PlaceDetailsResponseMessage (object sender, PlaceDetailsResponse placeDetailsResponse) : base (sender)
		{
			PlaceDetailsResponse = placeDetailsResponse;
		}

		public PlaceDetailsResponse PlaceDetailsResponse { get; private set; }
	}

}

