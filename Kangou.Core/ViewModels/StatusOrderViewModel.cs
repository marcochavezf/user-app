using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Views;
using Cirrious.CrossCore;
using System;
using System.Diagnostics;
using Kangou.Core;


namespace Kangou.Core
{
    public class StatusOrderViewModel 
		: MvxViewModel
    {
		public static bool HasBeenClosedByUser;

		public void Init(ActiveOrder activeOrder)
		{
			Debug.WriteLine ("ActiveORder: {0}", activeOrder);
			Status = "Buscando Kangou";
			Distance = "El kangou se encuentra a 7 km de su destino";
			ActiveOrder = activeOrder;

			HasBeenClosedByUser = false;

			ConnectionManager.On(SocketEvents.KangouGoingToPickUp, (data) => {
				ConnectionManager.Off(SocketEvents.KangouGoingToPickUp);
				SetKangouGoingToPickUp();
			});

			ConnectionManager.On(SocketEvents.KangouWaitingToPickUp, (data) => {
				ConnectionManager.Off(SocketEvents.KangouWaitingToPickUp);
				SetKangouWaitingToPickUp();
			});

			ConnectionManager.On(SocketEvents.KangouGoingToDropOff, (data) => {
				ConnectionManager.Off(SocketEvents.KangouGoingToDropOff);
				SetKangouGoingToDropOff();
			});

			ConnectionManager.On(SocketEvents.KangouWaitingToDropOff, (data) => {
				ConnectionManager.Off(SocketEvents.KangouWaitingToDropOff);
				SetKangouWaitingToDropOff();
			});

			ConnectionManager.On(SocketEvents.OrderSignedByClient, (data) => {
				ConnectionManager.Off(SocketEvents.OrderSignedByClient);
				SetOrderSignedByClient();
			});

			ConnectionManager.SocketDisconnected (delegate {
				Close(this);
			});
				
			switch (ActiveOrder.Status) {

			case StatusOrder.KangouGoingToPickUp:
				SetKangouGoingToPickUp();
				break;

			case StatusOrder.KangouWaitingToPickUp:
				SetKangouWaitingToPickUp ();
				break;

			case StatusOrder.KangouGoingToDropOff:
				SetKangouGoingToDropOff();
				break;

			case StatusOrder.KangouWaitingToDropOff:
				SetKangouWaitingToDropOff();
				break;

			case StatusOrder.OrderSignedByClient:
				SetOrderSignedByClient();
				break;

			case StatusOrder.OrderReviewed:
				SetOrderReviewed();
				break;
			}
		}

		public void TurnOffConnectionManager(){
			ConnectionManager.Off(SocketEvents.KangouGoingToPickUp);
			ConnectionManager.Off(SocketEvents.KangouWaitingToPickUp);
			ConnectionManager.Off(SocketEvents.KangouGoingToDropOff);
			ConnectionManager.Off(SocketEvents.KangouWaitingToDropOff);
			ConnectionManager.Off(SocketEvents.OrderSignedByClient);
		}

		private void SetKangouGoingToPickUp(){
			var message = "Un kangou va en camino a recoger";
			Status = message;
			ActiveOrder.Status = StatusOrder.KangouGoingToPickUp;
			StatusUpdated (message);
		}

		private void SetKangouWaitingToPickUp(){
			var message = "El kangou está esperando para recoger lo pedido";
			Status = message;
			Distance = "Ya está en el lugar para recoger";
			ActiveOrder.Status = StatusOrder.KangouWaitingToPickUp;
			StatusUpdated (message);
		}

		private void SetKangouGoingToDropOff(){
			var message = "El kangou está en camino a entregar";
			Status = message;
			ActiveOrder.Status = StatusOrder.KangouGoingToDropOff;
			StatusUpdated (message);
		}

		private void SetKangouWaitingToDropOff(){
			var message = "El kangou está esperando para entregar lo pedido";
			Status = message;
			Distance = "Favor de firmarle como recibido";
			ActiveOrder.Status = StatusOrder.KangouWaitingToDropOff;
			StatusUpdated (message);
		}

		private void SetOrderSignedByClient(){
			var message = "La orden ha finalizado";
			Status = message;
			Distance = "";
			ActiveOrder.Status = StatusOrder.OrderSignedByClient;
			StatusUpdated (message);
			Task.Run (delegate {
				ShowViewModel<ReviewViewModel> ();
			});
		}

		private void SetOrderReviewed(){
			Status = "La orden ha finalizado";
			Distance = "";
			ActiveOrder.Status = StatusOrder.OrderReviewed;
		}

		public ActiveOrder ActiveOrder { get; private set; }
		public event Action<string> StatusUpdated = delegate {};

		private string _status;
		public string Status {
			get { return _status; }
			set {
				_status = "Estatus:\n\n" + value;
				RaisePropertyChanged (() => Status);
			}
		}

		private string _distance;
		public string Distance {
			get { return _distance; }
			set {
				_distance = "\n" + value;
				RaisePropertyChanged (() => Distance);
			}
		}

		private string _lat;
		public string Lat { 
			get { return _lat; }
			set {
				_lat = value;
				RaisePropertyChanged (() => Lat);
			}
		}

		private string _lng;
		public string Lng { 
			get { return _lng; }
			set {
				_lng = value;
				RaisePropertyChanged (() => Lng);
			}
		}
    }
}
