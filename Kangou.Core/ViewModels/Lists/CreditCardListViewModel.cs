using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.CrossCore;
using Kangou.Core.Services.Location;
using Kangou.Core.Services.DataStore;
using System.Collections.Generic;
using Kangou.Core.ViewModels.ObserverMessages;
using Kangou.Core.ViewModels;

namespace Kangou.Core.ViewModels
{

	public class CreditCardListViewModel
		: MvxViewModel, IDelete
    {
		private readonly IDataService _dataService;
		private readonly IMvxMessenger _messenger;

		public CreditCardListViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_dataService = dataService;
			_messenger = messenger;
			CreditCardDataList = _dataService.AllCreditCardData ();
		}

		private List<CreditCardData> _creditCardDataList;
		public List<CreditCardData> CreditCardDataList
		{
			get { return _creditCardDataList; }
			set { _creditCardDataList = value; RaisePropertyChanged(() => CreditCardDataList); }
		}

		private MvxCommand _addCreditCardDataCommand;
		public ICommand AddCreditCardDataCommand {
			get {
				_addCreditCardDataCommand = _addCreditCardDataCommand ?? new MvxCommand (() =>{
					ShowViewModel<CreditCardViewModel>();
					Close(this);
				});
				return _addCreditCardDataCommand;
			}
		}

		public ICommand SelectDataCommand
		{
			get
			{
				return new MvxCommand<CreditCardData>(creditCardData => {
					_messenger.Publish (new CreditCardDataMessage (this, creditCardData));
					Close(this);
				});
			}
		}

		#region IDelete implementation
		public void DeleteData(int id)
		{
			_dataService.Delete (_dataService.GetCreditCardData(id));
			CreditCardDataList = _dataService.AllCreditCardData ();
		}

		public void DeleteDataByIndex(int index)
		{
			var creditCardDataList = _dataService.AllCreditCardData ();
			_dataService.Delete (creditCardDataList [index]);
			CreditCardDataList = _dataService.AllCreditCardData ();
		}
		#endregion

	}
}
