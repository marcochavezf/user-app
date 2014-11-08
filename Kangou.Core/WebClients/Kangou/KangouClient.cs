using System;
using Kangou.Core.Services.DataStore;
using System.Net;
using System.Text;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using System.Diagnostics;
using System.Collections.Generic;

namespace Kangou.Core.WebClients
{
	public class KangouClient
	{
		private readonly IMvxJsonConverter _jsonConverter;
		public KangouClient (IMvxJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
		}

		public void SendOrderData(ItemsData itemsData, PickUpData pickUpData, DropOffData dropOffData, CreditCardData _creditCardData, UserData userData,  Action<int> succesAction, Action<string> errorAction)
		{
			//var _endPointOrderData = "https://kangou.mx/rest_orders.json";
			var _endPointOrderData = "http://localhost:5000/orders";

			/* Preparing Data. */
			var request = (HttpWebRequest)WebRequest.Create(_endPointOrderData);
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			string postData = 
				"creditCardId=" 				+ _creditCardData.CardId +
				"&typeCardId=" 					+ _creditCardData.TypeCardId + 

				"&clientName=" 					+ userData.Name + 
				"&clientEmail=" 				+ userData.Email +
				"&clientPhoneNumber=" 			+ userData.PhoneNumber +

				"&listItems=" 					+ itemsData.Items +

				"&pickupLat=" 					+ pickUpData.Lat +
				"&pickupLng=" 					+ pickUpData.Lng +
				"&pickupStreet=" 				+ pickUpData.Street +
				"&pickupSublocality=" 			+ pickUpData.SubLocality +
				"&pickupLocality=" 				+ pickUpData.Locality +
				"&pickupAdministrativeArea=" 	+ pickUpData.AdministrativeArea +
				"&pickupCountry=" 				+ pickUpData.Country +
				"&pickupPostalCode=" 			+ pickUpData.PostalCode +
				"&pickupIsoCountryCode=" 		+ pickUpData.IsoCountryCode +
				"&pickupReferences=" 			+ pickUpData.References +
				"&pickupFullname=" 				+ pickUpData.FullName +

				"&dropoffLat=" 					+ dropOffData.Lat +
				"&dropoffLng=" 					+ dropOffData.Lng +
				"&dropoffStreet=" 				+ dropOffData.Street +
				"&dropoffSublocality=" 			+ dropOffData.SubLocality +
				"&dropoffLocality=" 			+ dropOffData.Locality +
				"&dropoffAdministrativeArea=" 	+ dropOffData.AdministrativeArea +
				"&dropoffCountry=" 				+ dropOffData.Country +
				"&dropoffPostalCode=" 			+ dropOffData.PostalCode +
				"&dropoffIsoCountryCode=" 		+ dropOffData.IsoCountryCode +
				"&dropoffReferences=" 			+ dropOffData.References +
				"&dropoffFullname=" 			+ dropOffData.FullName;

			Debug.WriteLine ("postData:{0}",postData);

			// Convert the string into a byte array.
			byte[] byteArray = Encoding.UTF8.GetBytes(postData);

			/* Sending Data to Server. */
			request.BeginGetRequestStream (asynchResultReq => {

				try
				{
					using (var stream = request.EndGetRequestStream(asynchResultReq))
					{
						System.Diagnostics.Debug.WriteLine("******* Writing: {0}",stream.CanWrite);

						// Write to the request stream.
						stream.Write(byteArray, 0, postData.Length);
						stream.Dispose ();

					}
				}
				catch (WebException ex)
				{
					var errorString = String.Format("ERROR Requesting Stream: '{0}' when making {1} request to {2}", ex.Message, request.Method, request.RequestUri.AbsoluteUri);
					Mvx.Error(errorString);
					errorAction(errorString);
				}

				/* Requesting Response from Server. */
				request.BeginGetResponse(asynchResultResp =>
					{
						try
						{
							using (var response = request.EndGetResponse(asynchResultResp))
							{
								using (var stream = response.GetResponseStream())
								{
									/* Getting Success response from server */
									var reader = new StreamReader(stream);
									var rootObj = _jsonConverter.DeserializeObject<RootObject>(reader.ReadToEnd());
									var confirmationNumber = rootObj.order.confirmation_number;
									System.Diagnostics.Debug.WriteLine("Confirmation number:{0}",confirmationNumber);
									succesAction(confirmationNumber);
								}
							}
						}
						catch (WebException ex)
						{
							var errorString = String.Format("ERROR Requesting Response: '{0}' when making {1} request to {2}", ex.Message, request.Method, request.RequestUri.AbsoluteUri);
							Mvx.Error(errorString);
							errorAction(errorString);
						}
					}, null);

			}, null);

		}



		public void GetActiveOrderList(int userId, Action<List<ActiveOrder>> succesAction, Action<string> errorAction)
		{
			var _endPointOrderData = "https://kangou.mx/rest_orders/1.json";

			/* Preparing Data. */
			var request = (HttpWebRequest)WebRequest.Create(_endPointOrderData);
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			string postData = 
				"user_id=" + userId;

			Debug.WriteLine ("postData:{0}",postData);

			// Convert the string into a byte array.
			byte[] byteArray = Encoding.UTF8.GetBytes(postData);

			/* Sending Data to Server. */
			request.BeginGetRequestStream (asynchResultReq => {
				try
				{
					using (var stream = request.EndGetRequestStream(asynchResultReq))
					{
						System.Diagnostics.Debug.WriteLine("******* Writing: {0}",stream.CanWrite);

						// Write to the request stream.
						stream.Write(byteArray, 0, postData.Length);
						stream.Dispose ();

					}
				}
				catch (WebException ex)
				{
					var errorString = String.Format("ERROR Requesting Stream: '{0}' when making {1} request to {2}", ex.Message, request.Method, request.RequestUri.AbsoluteUri);
					Mvx.Error(errorString);
					errorAction(errorString);
				}

				/* Requesting Response from Server. */
				request.BeginGetResponse(asynchResultResp =>
					{
						try
						{
							using (var response = request.EndGetResponse(asynchResultResp))
							{
								using (var stream = response.GetResponseStream())
								{
									/* Getting Success response from server */
									var reader = new StreamReader(stream);
									var jsonString = reader.ReadToEnd();
									Debug.WriteLine("reader: {0}",jsonString);
									var rootObj = _jsonConverter.DeserializeObject<ActiveOrderListRoot>(jsonString);
									succesAction(rootObj.active_order);
								}
							}
						}
						catch (WebException ex)
						{
							var errorString = String.Format("ERROR Requesting Response: '{0}' when making {1} request to {2}", ex.Message, request.Method, request.RequestUri.AbsoluteUri);
							Mvx.Error(errorString);
							errorAction(errorString);
						}
					}, null);

			}, null);

		}
	}
}

