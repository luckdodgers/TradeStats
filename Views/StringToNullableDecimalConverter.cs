using System;
using System.Globalization;
using System.Windows.Data;

namespace TradeStats.Views
{
    public class StringToNullableDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            var isNumeric = decimal.TryParse(value.ToString(), out var decimalValue);

            return isNumeric ? decimalValue : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var strValue = value.ToString();

            if (strValue == string.Empty || !decimal.TryParse(strValue, out var number))
                return null;

            return number;
        }
    }
}
