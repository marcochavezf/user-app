using System;

namespace Kangou.Core.WebClients
{
	public class Order
	{
		public int confirmation_number { get; set; }
	}

	public class RootObject
	{
		public Order order { get; set; }
	}
}

