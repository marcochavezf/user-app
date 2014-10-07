using System;

namespace Kangou.Core.Services.DataSender
{
	public interface IDataSenderService
	{
		T Deserialize<T>(string responseBody);
	}
}

