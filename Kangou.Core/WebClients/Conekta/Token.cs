using System;

namespace Kangou.Core.WebClients
{
	public class Token
	{
		public string id { get; set; }
		public bool livemode { get; set; }
		public bool used { get; set; }
		public string @object { get; set; }
	}
}

