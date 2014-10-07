using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace Kangou.Core.ViewModels
{

	/*************************
	 * PromoCode Message
	 * */
	public class PromoCodeMessage 
		: MvxMessage
	{
		public PromoCodeMessage (object sender, string promoCode) : base (sender)
		{
			PromoCode = promoCode;
		}
		public string PromoCode { get; private set; }
	}

	/*************************
	 * Promo Code View Model
	 * */

	public class PromoCodeViewModel 
		: MvxViewModel
    {

		private IMvxMessenger _messenger;

		public PromoCodeViewModel (IMvxMessenger messenger)
		{
			_messenger = messenger;
		}


		/* * * * * * * * * * * *
		 * Promo Code Property
		 */

		private string _promoCode;
		public string PromoCode { 
			get { return _promoCode; }
			set {
				_promoCode = value;
				RaisePropertyChanged (() => PromoCode);
			}
		}

		public void PublishData()
		{
			_messenger.Publish (new PromoCodeMessage (this, PromoCode));	
		}
	}
}
