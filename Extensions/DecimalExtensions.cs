using System;
using System.Collections.Generic;
using System.Text;

namespace TradeReportsConverter.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToTableViewString(this decimal number)
        {
            number = RoundNumber(number);
            var result = number.ToString();

            return result.Contains('.') ? result.TrimEnd('0').Replace('.', ',').TrimEnd(',') : result;
        }

        private static decimal RoundNumber(decimal number) =>

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
