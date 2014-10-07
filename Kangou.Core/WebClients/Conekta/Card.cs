using System;

namespace Kangou.Core.WebClients
{
	public class Card
	{
		public string id { get; set; }
		public int created_at { get; set; }
		public bool active { get; set; }
		public string last4 { get; set; }
		public string @object { get; set; }
		public string exp_month { get; set; }
		public string exp_year { get; set; }
		public string brand { get; set; }
		public string name { get; set; }
		public string customer_id { get; set; }
	}
}

