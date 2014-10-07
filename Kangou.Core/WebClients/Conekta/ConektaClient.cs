using System;
using System.Net.Http;
using ModernHttpClient;
using System.Text;
using System.Net.Http.Headers;
using Cirrious.CrossCore.Platform;
using System.Linq;

namespace Kangou.Core.WebClients
{
	public class ConektaClient
	{
		private const string AppHeader = "application/vnd.conekta-v0.3.0+json";
		private const string BaseUrl = "https://api.conekta.io/";
		//private const string PrivateKey = "key_rKkvTmf4E9t5BrK6";
		//private const string PublicKey = "key_eL9oHoCTCvHoBWtb";
		private string PrivateKey = AsciiToString(FromHex(Reverse(String.Format("{0}544366d6456{1}756b6","63b42724534793","7b6b427f59"))));
		private string PublicKey = AsciiToString(FromHex(Reverse(String.Format("{0}{1}4f693c{2}","26477524f684","67344534f68","456f59756b6"))));

		private readonly IMvxJsonConverter _jsonConverter;

		public ConektaClient (IMvxJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
			System.Diagnostics.Debug.WriteLine ("PrivateKey: {0}",PrivateKey);
			System.Diagnostics.Debug.WriteLine ("PublicKey: {0}",PublicKey);
		}

		public Client CreateClient(string name, string email, string phone, string shippingAddress, string card, string expMonth, string expYear, string cvc) {

			//Create Token
			var token = CreateToken (name, card, expMonth, expYear, cvc);

			//Send Post data to create client
			var jsonPostResponse = Post("customers", new {
				name,
				email,
				phone,
				cards = new string[]{ token.id },
				billing_address = shippingAddress,
				shipping_address = shippingAddress
			});
			System.Diagnostics.Debug.WriteLine (jsonPostResponse);
			var client = _jsonConverter.DeserializeObject<Client> (jsonPostResponse);
			return client;
		}
			
		private Token CreateToken(string name, string card, string expMonth, string expYear, string cvc){

			var endpoint = "tokens/create.js?callback=jsonp2" +
				"&card[number]=" 	+ card +
				"&card[name]=" 		+ name +
				"&card[exp_year]=" 	+ expYear +
				"&card[exp_month]=" + expMonth +
				"&card[cvc]=" 		+ cvc +
				"&_Version=0.3.0&_RaiseHtmlError=false&auth_token=" + PublicKey +
				"&conekta_client_user_agent={\"agent\":\"Conekta JavascriptBindings/0.3.0\"}";
			var getResponse = Get (endpoint);
			System.Diagnostics.Debug.WriteLine (getResponse);
			var jsonResponse = getResponse.Replace ("jsonp2(", "").Replace (")", "");
			var token = _jsonConverter.DeserializeObject<Token> (jsonResponse);
			return token;
		}
			

		#region Helpers
		private string Delete(string endpoint) {
			return GetClient().DeleteAsync(GetEndpoint(endpoint)).Result.Content.ReadAsStringAsync().Result;
		}

		private string Get(string endpoint) {
			return GetClient().GetStringAsync(GetEndpoint(endpoint)).Result;
		}

		private HttpClient GetClient(string Key = "") {
			var client = new HttpClient(new NativeMessageHandler());
			if (!Key.Equals(""))
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
					Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", Key))));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AppHeader));

			return client;
		}

		private Uri GetEndpoint(string endpoint) {
			return new Uri(string.Format("{0}{1}", BaseUrl, endpoint));
		}

		private string Post(string endpoint, object obj = null) {
			obj = obj ?? new object();
			return
				GetClient(PrivateKey)
					.PostAsync(GetEndpoint(endpoint), new StringContent(_jsonConverter.SerializeObject(obj), Encoding.UTF8, "application/json"))
					.Result.Content.ReadAsStringAsync()
					.Result;
		}

		private static byte[] FromHex(string hex)
		{
			byte[] raw = new byte[hex.Length / 2];
			for (int i = 0; i < raw.Length; i++)
			{
				raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
			}
			return raw;
		}

		private static string AsciiToString(byte[] bytes)
		{
			return string.Concat( bytes.Select(b => b <= 0x7f ? (char)b : '?') );
		}

		private static string Reverse( string s )
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse( charArray );
			return new string( charArray );
		}
		#endregion
	}
}

