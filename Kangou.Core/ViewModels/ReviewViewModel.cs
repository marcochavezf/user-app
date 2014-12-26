using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Xamarin.Socket.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics;


namespace Kangou.Core
{
	public class ReviewViewModel
		: MvxViewModel
    {
		private ActiveOrder _activeOrder;

		public void Init(ActiveOrder activeOrder)
		{
			_activeOrder = activeOrder;

			ConnectionManager.On  ( SocketEvents.ReviewAcceptedByClient, (data) => {
				ConnectionManager.Off  ( SocketEvents.ReviewAcceptedByClient );
				Debug.WriteLine("ReviewAcceptedByClient");
				ConnectionManager.ConnectionState = ConnectionStates.DISCONNECTED_BY_USER;
				ConnectionManager.Disconnect();
				Close(this);
				IsBusy = false;
			});
		}

		private bool _isBusy;
		public bool IsBusy
		{   
			get { return _isBusy; }
			set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
		}

		private string _commentsAboutClient;
		public string CommentsAboutClient
		{ 
			get { return _commentsAboutClient; }
			set { _commentsAboutClient = value; RaisePropertyChanged(() => CommentsAboutClient); }
		}

		private int _ratingAboutClient = 0;
		public int RatingAboutClient
		{ 
			get { return _ratingAboutClient; }
			set { _ratingAboutClient = value; RaisePropertyChanged(() => RatingAboutClient); }
		}

		private MvxCommand _acceptCommand;
		public ICommand AcceptCommand {
			get {
				_acceptCommand = _acceptCommand ?? new MvxCommand (DoAcceptCommand);
				return _acceptCommand;
			}
		}
		private void DoAcceptCommand ()
		{
			IsBusy = true;
			var jsonString = String.Format( "{{ \"{0}\": true, \"commentsAboutClient\": \"{1}\", \"ratingAboutClient\": {2}, \"orderId\": \"{3}\" }}", SocketEvents.ReviewAcceptedByClient, CommentsAboutClient.Replace(System.Environment.NewLine," "), RatingAboutClient, _activeOrder._id);
			ConnectionManager.Emit( SocketEvents.ReviewAcceptedByClient, jsonString);
		}

    }
}
