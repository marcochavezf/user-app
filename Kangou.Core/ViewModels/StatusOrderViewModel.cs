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
		public void Init(ActiveOrder activeOrder)
		{
			Debug.WriteLine ("ActiveORder: {0}", activeOrder);
			Status = "Buscando Kangou";
			Distance = "";
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
				SetOrderSignedByClient(activeOrder);
			});

			/* This methods are for updating view model information when 
			 * connection manager is trying to reconnect. */

			ConnectionManager.On(SocketEvents.Connected, (data) => {
				ConnectionManager.Emit( SocketEvents.ActiveOrder, ConnectionManager.OrderIdJsonString(activeOrder._id));
			});

			ConnectionManager.On (SocketEvents.ActiveOrder, (data) => {
				Debug.WriteLine("**************** On Active Order: {0}",data["status"].ToString());
				SetPropertiesViewModel (new ActiveOrder(){
					_id = data["_id"].ToString(),
					Status = data["status"].ToString(),
					Date = data["date"].ToString(),
					PickUpLat = Convert.ToDouble( data["pickup"]["lat"].ToString() ),
					PickUpLng = Convert.ToDouble( data["pickup"]["lng"].ToString() ),
					DropOffLat = Convert.ToDouble( data["dropoff"]["lat"].ToString() ),
					DropOffLng = Convert.ToDouble( data["dropoff"]["lng"].ToString() )
				});
			});

			SetPropertiesViewModel (activeOrder);
		}

		private void SetPropertiesViewModel (ActiveOrder activeOrder)
		{
			Debug.WriteLine("**************** Status: {0}",activeOrder.Status);
			switch (activeOrder.Status) {
			case StatusOrder.KangouGoingToPickUp:
				SetKangouGoingToPickUp ();
				break;
			case StatusOrder.KangouWaitingToPickUp:
				SetKangouWaitingToPickUp ();
				break;
			case StatusOrder.KangouGoingToDropOff:
				SetKangouGoingToDropOff ();
				break;
			case StatusOrder.KangouWaitingToDropOff:
				SetKangouWaitingToDropOff ();
				break;
			case StatusOrder.OrderSignedByClient:
				SetOrderSignedByClient (activeOrder);
				break;
			case StatusOrder.OrderReviewed:
				SetOrderReviewed ();
				break;
			}
		}

		public void TurnOffConnectionManager(){
			ConnectionManager.Off(SocketEvents.KangouGoingToPickUp);
			ConnectionManager.Off(SocketEvents.KangouWaitingToPickUp);
			ConnectionManager.Off(SocketEvents.KangouGoingToDropOff);
			ConnectionManager.Off(SocketEvents.KangouWaitingToDropOff);
			ConnectionManager.Off(SocketEvents.OrderSignedByClient);
			ConnectionManager.Off(SocketEvents.KangouPosition);

			ConnectionManager.Off(SocketEvents.Connected);
			ConnectionManager.Off(SocketEvents.ActiveOrder);
		}

		private void SetKangouGoingToPickUp(){
			var message = "Un kangou va en camino a recoger o comprar";
			Status = message;
			Distance = "Obteniendo posición del kangou...";
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
			Distance = "Obteniendo posición del kangou...";
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

		private void SetOrderSignedByClient(ActiveOrder activeOrder){
			TurnOffConnectionManager ();
			var message = "La orden ha finalizado";
			Status = message;
			Distance = "";
			ActiveOrder.Status = StatusOrder.OrderSignedByClient;
			StatusUpdated (message);
			Task.Run (delegate {
				ShowViewModel<ReviewViewModel> (activeOrder);
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
