using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TradeReportsConverter.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToTableViewPriceString(this decimal number)
        {
            number = RoundNumberForMergeTrades(number);
            var result = number.ToString(CultureInfo.InvariantCulture);

            return result.Contains('.') ? result.TrimEnd('0').Replace('.', ',').TrimEnd(',') : result;
        }

        public static string TwoDigitsAfterDotRoundUp(this decimal number)
        {
            number = decimal.Round(number, 2);
            var result = number.ToString(CultureInfo.InvariantCulture);

            return result.Contains('.') ? result.TrimEnd('0').Replace('.', ',').TrimEnd(',') : result;
        }

        private static decimal RoundNumberForMergeTrades(decimal number) =>

        number switch
        {
            < 0.1M => decimal.Round(number, 7),
            < 1 => decimal.Round(number, 5),
            < 10 => decimal.Round(number, 4),
            < 100 => decimal.Round(number, 2),
            < 1000 => decimal.Round(number, 1),
            _ => decimal.Round(number, 0)
        };
    }
}
