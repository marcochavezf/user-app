using System;
using Cirrious.CrossCore.Converters;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Kangou.Core
{
	public class CreditCardHideValueConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return StringFormater.HideCreditCardNumber (value.ToString());
		}

	}
}

