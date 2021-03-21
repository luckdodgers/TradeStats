using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TradeReportsConverter.Exceptions;
using TradeStats.Models.Domain;
using System.Windows.Media;

namespace TradeStats.Extensions
{  
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };

        public static SolidColorBrush ToBrush(this string HexColorString) => (SolidColorBrush)new BrushConverter().ConvertFrom(HexColorString);
    }
}
