using System;

namespace Kangou.Core
{
	public class ActiveOrder
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Format {
			get { return String.Format("Orden {0} - {1}", Id, Date); }
		}
	}
}

