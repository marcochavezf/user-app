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

namespace Kangou.Core
{


	public class ActiveOrdersListViewModel
		: MvxViewModel
    {

		private readonly IDataService _dataService;
		private readonly IMvxMessenger _messenger;

		public ActiveOrdersListViewModel (IDataService dataService, IMvxMessenger messenger)
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

		public ICommand SelectDataCommand
		{
			get
			{
				return new MvxCommand<DropOffData>(dropOffData => {
					_messenger.Publish (new DropOffDataMessage (this, dropOffData));
					Close(this);
				});
			}
		}
	}
}
