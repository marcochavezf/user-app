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


	public class PickUpListViewModel
		: MvxViewModel, IDelete
    {

		private readonly IDataService _dataService;
		private readonly IMvxMessenger _messenger;

		public PickUpListViewModel (IDataService dataService, IMvxMessenger messenger)
		{
			_dataService = dataService;
			_messenger = messenger;
			PickUpDataList = _dataService.AllPickUpData ();
		}

		private List<PickUpData> _pickUpDataList;
		public List<PickUpData> PickUpDataList
		{
			get { return _pickUpDataList; }
			set { _pickUpDataList = value; RaisePropertyChanged(() => PickUpDataList); }
		}

		private MvxCommand _AddPickUpDataCommand;
		public ICommand AddPIckUpDataCommand {
			get {
				_AddPickUpDataCommand = _AddPickUpDataCommand ?? new MvxCommand (() =>{
					ShowViewModel<PickUpViewModel>();
					Close(this);
				});
				return _AddPickUpDataCommand;
			}
		}

		public ICommand SelectDataCommand
		{
			get
			{
				return new MvxCommand<PickUpData>(pickUpData => {
					_messenger.Publish (new PickUpDataMessage (this, pickUpData));
					Close(this);
				});
			}
		}

		#region IDelete implementation
		public void DeleteData(int id)
		{
			_dataService.Delete (_dataService.GetPickUpData(id));
			PickUpDataList = _dataService.AllPickUpData ();
		}


		public void DeleteDataByIndex(int index)
		{
			var pickUpDataList = _dataService.AllPickUpData ();
			_dataService.Delete (pickUpDataList [index]);
			PickUpDataList = _dataService.AllPickUpData ();
		}
		#endregion
	}
}
