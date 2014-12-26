using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics;
using Xamarin.Socket.IO;
using System.Collections.Generic;

namespace Kangou.Core
{
	public enum ConnectionStates
	{
		USER_WANTS_TO_BE_CONNECTED, CONNECTED_BY_SERVER, DISCONNECTED_BY_USER 
	}

	public class ConnectionManager
	{
		private static string _userId;
		private const string _endPoint 	= "kangou.herokuapp.com";
		public SocketIO Socket { get; set; }
		private static ConnectionManager instance;
		private static volatile ConnectionStates _connectionState;
		private volatile static Dictionary<string, Action> _actionsToPerform;
		
		private ConnectionManager() {
			Socket =  new SocketIO (host : _endPoint);
			ConnectionState = ConnectionStates.DISCONNECTED_BY_USER;
			_actionsToPerform = new Dictionary<string, Action> ();

			var connectionState = ConnectionState;

			Socket.SocketClosedByError += (arg1, arg2) => {
				Instance.TryingToReconnect (true);
				Debug.WriteLine("handleConnectError IsConectedByUser: {0}",ConnectionState);

				if(ConnectionState == ConnectionStates.USER_WANTS_TO_BE_CONNECTED
				|| ConnectionState == ConnectionStates.CONNECTED_BY_SERVER) {
					Debug.WriteLine("ConnectAgain because of SocketClosedByError");

					Connect (_userId);
				}
			};

			Socket.TimedOut += () => {
				Instance.TryingToReconnect (true);

				if(ConnectionState == ConnectionStates.USER_WANTS_TO_BE_CONNECTED
					|| ConnectionState == ConnectionStates.CONNECTED_BY_SERVER) {
					Debug.WriteLine("ConnectAgain");
					Connect (_userId);
				}
			};

			Socket.SocketFailedToConnect += (obj) => {
				Instance.TryingToReconnect (true);
				Debug.WriteLine("handleConnectError SocketFailedToConnect: {0}",connectionState);

				if(ConnectionState == ConnectionStates.USER_WANTS_TO_BE_CONNECTED
					|| ConnectionState == ConnectionStates.CONNECTED_BY_SERVER) {
					Debug.WriteLine("ConnectAgain");
					Connect (_userId);
				}
			};

			Socket.On (SocketEvents.Connected, ActionsToPerformWhenConnected());
		}

		public static void FailedToConnect(Action action){

		}

		public static void SocketDisconnected(Action action){
			Instance.Socket.SocketDisconnected += delegate {
				action();
			};
		}

		public static ConnectionStates ConnectionState {
			get { return _connectionState; }
			set { _connectionState = value; }
		}

		public static void On(string name, Action <JToken> handler){
			Instance.Socket.On (name, handler);
		}

		public static void Off(string name){
			Instance.Socket.Off (name);
			var isRemoved = _actionsToPerform.Remove (name);
			Debug.WriteLine ("{0} is removed: {1}",name,isRemoved);

			if (name == SocketEvents.Connected) 
				Instance.Socket.On (SocketEvents.Connected, ActionsToPerformWhenConnected());
		}

		private static Action<JToken> ActionsToPerformWhenConnected(){
			return (data) => {
				Debug.WriteLine ("**** Performing events ****");
				foreach (KeyValuePair<string, Action> entry in _actionsToPerform) {
					Debug.WriteLine (" Action performed: {0}", entry.Key);
					entry.Value ();
				}
			};
		}

		public static void Connect(string userId){
			_userId = userId;
			Instance.Socket.ConnectAsync (new Dictionary<string, string>()
				{
					{ "typeUser", "kangouClient" },
					{ "userId", _userId },
				});
		}
				
		public static void Disconnect(){
			Instance.Socket.Disconnect ();
		}
			
		public static void Emit (string name, IEnumerable args){

			Debug.WriteLine ("***** Adding: {0}",name);
			_actionsToPerform.Remove (name);
			_actionsToPerform.Add (name, delegate {
				Instance.Socket.Emit (name, args);
			});

			if (Instance.Socket.Connected) {
				Instance.TryingToReconnect (false);
				Instance.Socket.Emit (name, args);
			} else {
				Instance.TryingToReconnect (true);
				Connect (_userId);
			}
		}

		public static ConnectionManager Instance
		{
			get 
			{
				if (instance == null)
				{
					instance = new ConnectionManager();
				}
				return instance;
			}
		}

		public static string OrderIdJsonString(string orderId){
			return String.Format( "{{ \"id\": \"{0}\" }}", orderId);
		}

		public event Action<bool> TryingToReconnect = delegate {};

	}
}