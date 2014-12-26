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

namespace Kangou.Core.ViewModels
{


	public class DropOffListViewModel
		: MvxViewModel, IDelete
    {

		private readonly IDataService _dataService;
		private readonly IMvxMessenger _messenger;

		public DropOffListViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_dataService = dataService;
			_messenger = messenger;
			DropOffDataList = _dataService.AllDropOffData ();
		}

		private List<DropOffData> _dropOffDataList;
		public List<DropOffData> DropOffDataList
		{
			get { return _dropOffDataList; }
			set { _dropOffDataList = value; RaisePropertyChanged(() => DropOffDataList); }
		}

		private MvxCommand _AddDropOffDataCommand;
		public ICommand AddDropOffDataCommand {
			get {
				_AddDropOffDataCommand = _AddDropOffDataCommand ?? new MvxCommand (() =>{
					ShowViewModel<DropOffViewModel>();
				});
				return _AddDropOffDataCommand;
			}
		}

		public ICommand SelectDataCommand
		{
			get
			{
				return new MvxCommand<DropOffData>(dropOffData => {
					_messenger.Publish (new DropOffDataMessage (this, dropOffData));
					if(RegisterOrderViewModel.isStraightNavigation){
						if (_dataService.CountCreditCardData > 0) 
							ShowViewModel <CreditCardListViewModel> ();
						else
							ShowViewModel <CreditCardViewModel> ();
					} else {
						Close(this);
					}
				});
			}
		}

		#region IDelete implementation
		public void DeleteData(int id)
		{
			_dataService.Delete (_dataService.GetDropOffData(id));
			DropOffDataList = _dataService.AllDropOffData ();
		}

		public void DeleteDataByIndex(int index)
		{
			var dropOffDataList = _dataService.AllDropOffData ();
			_dataService.Delete (dropOffDataList [index]);
			DropOffDataList = _dataService.AllDropOffData ();
		}
		#endregion
	}
}
