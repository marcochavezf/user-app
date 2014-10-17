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
		public string Street { get; set; }
		public string SubLocality { get; set; }
		public string Locality { get; set; }
		public string AdministrativeArea { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string IsoCountryCode { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string AddressToDisplay { get; set; }
    }
}
