using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;

namespace Kangou.Core.ViewModels.ObserverMessages
{

	/*************************
	 * Pick Up Data Message
	 * */
	public class PickUpDataMessage 
		: MvxMessage
	{
		public PickUpDataMessage (object sender, PickUpData pickUpData) : base (sender)
		{
			PickUpData = pickUpData;
		}
		public PickUpData PickUpData { get; private set; }
	}
	
}
