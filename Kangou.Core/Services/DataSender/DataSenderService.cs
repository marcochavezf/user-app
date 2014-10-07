using System;
using Cirrious.CrossCore.Platform;
using ModernHttpClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Kangou.Core.Services.DataSender
{
	public class DataSenderService
		: IDataSenderService
	{
		private readonly IMvxJsonConverter _jsonConverter;

		public DataSenderService(IMvxJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
		}

		public T Deserialize<T>(string responseBody)
		{
			var toReturn = _jsonConverter.DeserializeObject<T>(responseBody);
			return toReturn;
		}

	}
}

