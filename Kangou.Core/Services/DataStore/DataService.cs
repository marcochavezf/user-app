using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Diagnostics;

namespace Kangou.Core.Services.DataStore
{
    public class DataService : IDataService
    {
        private readonly ISQLiteConnection _connection;

        public DataService(ISQLiteConnectionFactory factory)
        {
            _connection = factory.Create("kangou.sql");
            _connection.CreateTable<DropOffData>();
			_connection.CreateTable<PickUpData>();
			_connection.CreateTable<CreditCardData>();
			_connection.CreateTable<UserData>();
        }


		/*******************
		 * Drop Off Data
		 * */

		public List<DropOffData> AllDropOffData()
        {
			return _connection
				.Table<DropOffData> ()
				.OrderBy(x => x.Id)
				.ToList ();
        }

        public int CountDropOffData
        {
            get
            {
				return _connection
					.Table<DropOffData> ()
					.Count ();
            }
        }

		public void Add(DropOffData collectedItem)
        {
            _connection.Insert(collectedItem);
        }

		public void Delete(DropOffData collectedItem)
        {
            _connection.Delete(collectedItem);
        }

		public void Update(DropOffData collectedItem)
        {
            _connection.Update(collectedItem);
        }

		public DropOffData GetDropOffData(int id)
        {
			return _connection.Get<DropOffData>(id);
        }


		/*******************
		 * Pick Up Data
		 * */


		public List<PickUpData> AllPickUpData()
		{
			return _connection
				.Table<PickUpData> ()
				.OrderBy(x => x.Id)
				.ToList ();
		}

		public int CountPickUpData {
			get
			{
				return _connection
					.Table<PickUpData> ()
					.Count ();
			}
		}

		public void Add(PickUpData collectedItem)
		{
			_connection.Insert(collectedItem);
		}

		public void Delete(PickUpData collectedItem)
		{
			_connection.Delete(collectedItem);
		}

		public void Update(PickUpData collectedItem)
		{
			_connection.Update(collectedItem);
		}

		public PickUpData GetPickUpData(int id)
		{
			return _connection.Get<PickUpData>(id);
		}


		/*******************
		 * Credit Card Data
		 * */


		public List<CreditCardData> AllCreditCardData()
		{
			return _connection
				.Table<CreditCardData> ()
				.OrderBy(x => x.Id)
				.ToList ();
		}

		public int CountCreditCardData {
			get
			{
				return _connection
					.Table<CreditCardData> ()
					.Count ();
			}
		}

		public void Add(CreditCardData collectedItem)
		{
			_connection.Insert(collectedItem);
		}

		public void Delete(CreditCardData collectedItem)
		{
			_connection.Delete(collectedItem);
		}

		public void Update(CreditCardData collectedItem)
		{
			_connection.Update(collectedItem);
		}

		public CreditCardData GetCreditCardData(int id)
		{
			return _connection.Get<CreditCardData>(id);
		}


		/*******************
		 * User Data
		 * */
		public void AddOrUpdate(UserData userDataParam)
		{
			var userData = GetUserData ();
			if (userData == null) {
				Debug.WriteLine ("Adding User Data");
				_connection.Insert (userDataParam);
			} else {
				Debug.WriteLine ("Updating User Data");
				userDataParam.Id = userData.Id;
				_connection.Update (userDataParam);
			}
		}

		public UserData GetUserData()
		{
			return _connection
				.Table<UserData> ()
				.OrderBy (x => x.Id)
				.FirstOrDefault<UserData> ();
		}
    }
}
