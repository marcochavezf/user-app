using System;
using Kangou.Core.Services.DataStore;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using System.Text;
using System.Threading;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kangou.Core
{
	public class DataSender
	{

		public static void SendOrderData(ItemsData itemsData, PickUpData pickUpData, DropOffData dropOffData, UserData userData, String idCreditCard, Action<string> succesAction, Action<string> errorAction)
		{

			/* Preparing Data. */
			var request = (HttpWebRequest)WebRequest.Create("http://kangou.mx/rest_orders.json");
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			idCreditCard = idCreditCard ?? "cash";

			string postData = 
				"credit_card_id=" 		+ idCreditCard +
				"&list_items=" 			+ itemsData.Items +
				"&pickup_address=" 		+ pickUpData.Street +
				"&pickup_references=" 	+ pickUpData.References +
				"&pickup_fullname=" 	+ pickUpData.FullName +
				"&dropoff_address=" 	+ dropOffData.Street +
				"&dropoff_references=" 	+ dropOffData.References +
				"&dropoff_fullname=" 	+ dropOffData.FullName +
				"&client_name=" 		+ userData.Name + 
				"&client_email=" 		+ userData.Email +
				"&client_phone_number=" + userData.PhoneNumber;
				
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
									var reader = new StreamReader(stream);
									succesAction(reader.ReadToEnd());
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

