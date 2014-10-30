using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;
using Kangou.Core.ViewModels;

namespace Kangou.Core
{

	/*************************
	 * Reset Order Data Message
	 * */
	public class ChangeStateViewToggledMessage 
		: MvxMessage
	{
		public ChangeStateViewToggledMessage (object sender, TypeRootViewOpened typeViewOpened) : base (sender)
		{
			TypeViewOpened = typeViewOpened;
		}

		public TypeRootViewOpened TypeViewOpened { get; private set; }
	}
	
}
