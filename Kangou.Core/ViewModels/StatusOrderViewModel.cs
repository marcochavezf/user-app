using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Views;
using Cirrious.CrossCore;
using System;
using System.Diagnostics;
using Kangou.Core;


namespace KangouMessenger.Core
{
    public class StatusOrderViewModel 
		: MvxViewModel
    {
		public void Init(ActiveOrder activeOrder)
		{
			Debug.WriteLine ("ActiveORder: {0}", activeOrder);
			Status = "Buscando Kangou";
			Distance = "El kangou se encuentra a 7 km de su destino";
			ActiveOrder = activeOrder;

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

		private void SetKangouGoingToPickUp(){
			Status = "Un kangou va en camino a recoger";
		}

		private void SetKangouWaitingToPickUp(){
			Status = "El kangou está esperando para recoger lo pedido";
			Distance = "Ya está en el lugar para recoger";
		}

		private void SetKangouGoingToDropOff(){
			Status = "El kangou está en camino a entregar";
		}

		private void SetKangouWaitingToDropOff(){
			Status = "El kangou está esperando para entregar lo pedido";
			Distance = "Favor de firmarle como recibido";
		}

		private void SetOrderSignedByClient(){
			Status = "La orden ha finalizado";
			Distance = "";
			ShowViewModel<ReviewViewModel> ();
		}

		private void SetOrderReviewed(){
			Status = "La orden ha finalizado";
			Distance = "";
		}

		public ActiveOrder ActiveOrder { get; private set; }

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
