using System.Collections.Generic;

namespace Kangou.Core.Services.DataStore
{
    public interface IDataService
    {
		List<DropOffData> AllDropOffData();
        int CountDropOffData { get; }
		void Add(DropOffData collectedItem);
		void Delete(DropOffData collectedItem);
		void Update(DropOffData collectedItem);
		DropOffData GetDropOffData(int id);

		List<PickUpData> AllPickUpData();
		int CountPickUpData { get; }
		void Add(PickUpData collectedItem);
		void Delete(PickUpData collectedItem);
		void Update(PickUpData collectedItem);
		PickUpData GetPickUpData(int id);

		List<CreditCardData> AllCreditCardData();
		int CountCreditCardData { get; }
		void Add(CreditCardData collectedItem);
		void Delete(CreditCardData collectedItem);
		void Update(CreditCardData collectedItem);
		CreditCardData GetCreditCardData(int id);
    }
}