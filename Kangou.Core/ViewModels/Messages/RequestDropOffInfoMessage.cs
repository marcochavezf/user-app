using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Kangou.Core.ViewModels.ObserverMessages
{
	public class RequestDropOffInfoMessage
		: MvxMessage
	{
		public RequestDropOffInfoMessage (object sender) : base (sender)
		{
		}
	}
}

