using System;
using System.Text.RegularExpressions;

namespace Kangou.Core
{
	public class StringFormater
	{
		public static string FormatCreditCardNumber(string creditCardNumber)
		{
			string newCreditCardNumber = "";
			string oldCreditCardNumber = creditCardNumber.Replace(" ", string.Empty);
			for (int i = 0; i < oldCreditCardNumber.Length; i++)
			{
				if(i % 4 == 0 && i > 0)
				{
					newCreditCardNumber += " ";
				}

				var number = oldCreditCardNumber[i].ToString();
				string pattern = @"^[0-9]+$";
				Regex regex = new Regex(pattern);

				if(regex.IsMatch(number))
					newCreditCardNumber += number;

			}

			return newCreditCardNumber;
		}

		public static string FormatExpirationDate(string expirationDate)
		{
			string newExpirationDate = "";
			string oldExpirationDate = expirationDate.Replace(" ", string.Empty);
			for (int i = 0; i < oldExpirationDate.Length; i++)
			{
				var numberString = oldExpirationDate[i].ToString();

				//Discard checking with slash character has been added
				if(i==2 && numberString == "/" && oldExpirationDate.Length == 3)
				{
					continue;
				}

				//Discard checking with slash character
				if(i==2 && numberString == "/")
				{
					newExpirationDate += numberString;
					continue;
				}

				//Check if character entered is number
				string pattern = @"^[0-9]+$";
				Regex regex = new Regex(pattern);
				if(!regex.IsMatch(numberString))
					continue;

				var number = Convert.ToInt32(numberString);

				//Check first digit entered
				if(oldExpirationDate.Length == 1 && i == 0)
				{
					if(number > 1)
						numberString = "0" + numberString;
				}

				//Check second digit entered
				if(oldExpirationDate.Length == 2 && i == 1)
				{
					if(oldExpirationDate[i-1].ToString() == "1" && number > 2)
						continue;
				}

				//Check if user entered third digit
				if(oldExpirationDate.Length > 2 && i == 2)
				{
					newExpirationDate += "/";
				}

				newExpirationDate += numberString;
			}
			return newExpirationDate;
		}

		public static string HideCreditCardNumber(string creditCardNumber)
		{
			return "**** **** **** " + creditCardNumber.Substring (creditCardNumber.Length - 4);
		}
	}
}

