using System;
using System.Collections.Generic;

namespace Kangou.Core.WebClients
{
	public class Client
	{
		public string id { get; set; }
		public string email { get; set; }
		public string name { get; set; }
		public string phone { get; set; }
		public bool livemode { get; set; }
		public string default_card_id { get; set; }
		public string @object { get; set; }
		public int created_at { get; set; }
		public List<Card> cards { get; set; }
		public object subscription { get; set; }
	}
}

