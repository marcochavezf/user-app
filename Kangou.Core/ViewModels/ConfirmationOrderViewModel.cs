using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Kangou.Core.Services.DataStore;
using Kangou.Core.ViewModels.ObserverMessages;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * Items View Model
	 * */

	public class ConfirmationOrderViewModel 
		: MvxViewModel
    {

		private readonly IMvxMessenger _messenger;
		public ConfirmationOrderViewModel (IMvxMessenger messenger)
		{
			_messenger = messenger;
		}

		/* * * * * * * * * * * * * * * * * * * * * * * * 
		 * Pick Up Address Command
		 */

		private MvxCommand _resetOrderDataCommand;
		public ICommand ResetOrderDataCommand {
			get {
				_resetOrderDataCommand = _resetOrderDataCommand ?? new MvxCommand (()=>
					{
						_messenger.Publish (new ResetOrderDataMessage (this));
						Close(this);
					});
				return _resetOrderDataCommand;
			}
		}
	}
}
