using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Kangou.Core.Services.DataStore
{
    public class ItemsData
    {
		public string Items { get; set; }
		public bool IsAPurchase { get; set; }
    }
}
