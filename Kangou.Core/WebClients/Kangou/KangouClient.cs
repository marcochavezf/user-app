﻿using System;
using Kangou.Core.Services.DataStore;
using System.Net;
using System.Text;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Kangou.Core.WebClients
{
	public class KangouClient
	{
		private readonly IMvxJsonConverter _jsonConverter;

		public KangouClient (IMvxJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
		}

		public void SendOrderData(ItemsData itemsData, DropOffData dropOffData, UserData userData, String idCreditCard, Action<string> succesAction, Action<string> errorAction)
		{

			/* Preparing Data. */
			var request = (HttpWebRequest)WebRequest.Create("https://kangou.mx/rest_orders.json");
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			var pickupAddress 	 = "";
			var pickupReferences = "";
			var pickupFullname 	 = "";

			idCreditCard = idCreditCard ?? "cash";

			if (itemsData.PickUpData != null) 
			{
				pickupAddress 	 = itemsData.PickUpData.Address;
				pickupReferences = itemsData.PickUpData.References;
				pickupFullname 	 = itemsData.PickUpData.FullName;
			}

			string postData = 
				"credit_card_id=" 		+ idCreditCard +
				"&list_items=" 			+ itemsData.Items +
				"&pickup_address=" 		+ pickupAddress +
				"&pickup_references=" 	+ pickupReferences +
				"&pickup_fullname=" 	+ pickupFullname +
				"&dropoff_address=" 	+ dropOffData.Address +
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
									/* Getting Success response from server */
									var reader = new StreamReader(stream);
									var rootObj = _jsonConverter.DeserializeObject<RootObject>(reader.ReadToEnd());
									var confirmationNumber = rootObj.order.confirmation_number;
									System.Diagnostics.Debug.WriteLine("Confirmation number:{0}",confirmationNumber);
									succesAction(confirmationNumber.ToString());
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

