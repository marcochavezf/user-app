using System;

namespace Kangou.Core
{
	public class ActiveOrder
	{
		public static string LAST_ORDER_PLACED_ID = "";

		public string _id { get; set; }
		public string Status { get; set; }
		public string Date { get; set; }
		public string Format {
			get { 
				var stringId = _id;
				if (stringId.Length > 4)
					stringId = _id.Substring (_id.Length - 4);

				return String.Format("Orden {0} - {1}", stringId.ToUpper(), Date); 
			}
		}
	}
}

