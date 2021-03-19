using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeReportsConverter.Exceptions;
using TradeStats.Exceptions;
using TradeStats.Models.Domain;

namespace TradeStats.Extensions
{
    public static class EnumParseExtension
    {
        private static readonly Dictionary<RawCurrencies, Currency> CurrencyDict = new Dictionary<RawCurrencies, Currency>()
        {
            [RawCurrencies.XBT] = Currency.BTC,
        };
        private static IEnumerable<string> _formattedCurrencies => Enum.GetNames(typeof(Currency));
        private static IEnumerable<string> _rawCurrencies => Enum.GetNames(typeof(RawCurrencies));

        public static TradeSide ParseToSideEnum(this string rawValue)
        {
            var formattedValue = rawValue.ToLowerInvariant();

            if (formattedValue == TradeSide.Buy.ToString().ToLowerInvariant())
                return TradeSide.Buy;

            else if (formattedValue == TradeSide.Sell.ToString().ToLowerInvariant())
                return TradeSide.Sell;

            throw new ParseException($"Value \"{rawValue}\" can't be formatted to {nameof(TradeSide)} enum");
        }   

        public static (Currency, Currency) GetCurrencyEnumPair(this string rawValue)
        {
            var formattedStringCurrencies = new List<string>(2);

            foreach (var formatted in _formattedCurrencies)
            {
                if (rawValue.Contains(formatted))
                    formattedStringCurrencies.Add(formatted);
            }

            if (formattedStringCurrencies.Count < 2)
            {
                foreach (var raw in _rawCurrencies)
                {
                    if (rawValue.Contains(raw))
                    {
                        var formattedValue = CurrencyDict[(RawCurrencies)Enum.Parse(typeof(RawCurrencies), raw)];
                        formattedStringCurrencies.Add(formattedValue.ToString());
                    }
                }
            }

            if (formattedStringCurrencies.Count != 2)
                throw new FormatPatternException($"Can't format value=\"{rawValue}\" into {nameof(Currency)} enum");

            int index_1 = rawValue.IndexOf(formattedStringCurrencies[0]);
            int index_2 = rawValue.IndexOf(formattedStringCurrencies[1]);

            var currency_1 = (Currency)Enum.Parse(typeof(Currency), formattedStringCurrencies[0]);
            var currency_2 = (Currency)Enum.Parse(typeof(Currency), formattedStringCurrencies[1]);

            if (index_1 < index_2)
                return (currency_1, currency_2);

            else return (currency_2, currency_1);
        }
    }
}
