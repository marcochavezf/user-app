using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;

namespace Kangou.Core.ViewModels.ObserverMessages
{

	/*************************
	 * Reset Order Data Message
	 * */
	public class ResetOrderDataMessage 
		: MvxMessage
	{
		public ResetOrderDataMessage (object sender) : base (sender)
		{
		}
	}
	
}
