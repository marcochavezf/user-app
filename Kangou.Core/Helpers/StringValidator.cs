using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Kangou.Core
{
	public class StringValidator
	{
		public static bool IsValidCreditCard(string creditCardNumber)
		{
			//Removing white spaces
			creditCardNumber = creditCardNumber.Replace (" ", string.Empty);

			var cardNumberLenght = creditCardNumber.Length;

			if (cardNumberLenght >= 12) {
				char[] c = creditCardNumber.ToCharArray ();
				Array.Reverse(c);
				int[] cint = new int[cardNumberLenght];
				for (int i = 0; i < cardNumberLenght; i++) {
					if (i % 2 == 1) {
						cint [i] = Convert.ToInt32 (c [i].ToString ()) * 2;
						if (cint [i] > 9)
							cint [i] = 1 + cint [i] % 10;
					} else {
						cint [i] = Convert.ToInt32 (c [i].ToString ());
					}
				}
				int sum = 0;
				for (int i = 0; i < cardNumberLenght; i++) {
					sum += cint [i];
				}
					
				if (sum % 10 == 0)
					return true;
				else
					return false;
			} else
				return false;
		}

		public static void TestCreditCards(){

			string[] valueTesting = {

				"4716673197282560",
				"4716624450370675",
				"4485134705877549",
				"4379397154261551",
				"4539786940457167",
				"5343004350226335",
				"5470758276460030",
				"5497351786683512",
				"5452313748592663",
				"5347643367984638",
				"6011427860632180",
				"6011349691564699",
				"6011600761792705",
				"6011291708119101",
				"6011365234985803",
				"371367832123776",
				"378419243517733",
				"378495788657431",
				"376979007416093",
				"377702294681859"
			};

			foreach (string card in valueTesting) {
					System.Diagnostics.Debug.WriteLine ("isValid: {0} card:{1}",IsValidCreditCard(card),card);
			}
		}

		private static Regex ValidEmailRegex = CreateValidEmailRegex();

		/// <summary>
		/// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
		/// </summary>
		/// <returns></returns>
		private static Regex CreateValidEmailRegex()
		{
			string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

			return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
		}

		public static bool IsValidEmail(string emailAddress)
		{
			bool isValid = ValidEmailRegex.IsMatch(emailAddress);

			return isValid;
		}

	}
}

