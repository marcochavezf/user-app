using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;

namespace Kangou.Core.ViewModels.ObserverMessages
{

	/*************************
	 * Drop Off Message
	 * */
	public class DropOffDataMessage 
		: MvxMessage
	{
		public DropOffDataMessage (object sender, DropOffData dropOffData) : base (sender)
		{
			DropOffData = dropOffData;
		}
		public DropOffData DropOffData { get; private set; }
	}
	
}
