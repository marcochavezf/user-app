using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;

namespace Kangou.Core.ViewModels.ObserverMessages
{

	/*************************
	 * Items Message
	 * */
	public class ItemsDataMessage 
		: MvxMessage
	{
		public ItemsDataMessage (object sender, ItemsData itemsData) : base (sender)
		{
			ItemsData = itemsData;
		}
		public ItemsData ItemsData { get; private set; }
	}
	
}
