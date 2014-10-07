using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Kangou.Core.Services.DataStore
{
    public class PickUpData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string FullName { get; set; }
		public string References { get; set; }
		public string Address { get; set; }
    }
}
