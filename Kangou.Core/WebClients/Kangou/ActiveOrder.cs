using System;

namespace Kangou.Core
{
	public class ActiveOrder
	{
		public static int LAST_ORDER_PLACED_ID = -1;

		public int Id { get; set; }
		public string Status { get; set; }
		public string Date { get; set; }
		public string Format {
			get { return String.Format("Orden {0} - {1}", Id, Date); }
		}
	}
}

