using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Kangou.Core.Services.DataStore
{
    public class CreditCardData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
		public string CreditCardNumber { get; set; }
		public string CardId { get; set; }
		public string TypeCardId { get; set; }
    }
}
