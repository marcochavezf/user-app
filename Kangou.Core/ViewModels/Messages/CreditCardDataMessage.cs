using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Text.RegularExpressions;
using Kangou.Core.Services.DataStore;

namespace Kangou.Core.ViewModels.ObserverMessages
{

	/*************************
	 * Credit Card Message
	 * */
	public class CreditCardDataMessage 
		: MvxMessage
	{
		public CreditCardDataMessage (object sender, CreditCardData creditCardData) : base (sender)
		{
			CreditCardData = creditCardData;
		}
		public CreditCardData CreditCardData { get; private set; }
	}
	
}