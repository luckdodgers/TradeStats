using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TradeReportsConverter.Exceptions;
using TradeStats.Models.Domain;

namespace TradeStats.Extensions
{  
    public static class StringFormatExtensions
    {
        private static readonly Dictionary<RawCurrencies, Currency> CurrencyDict = new Dictionary<RawCurrencies, Currency>()
        {
            [RawCurrencies.XBT] = Currency.BTC,
            [RawCurrencies.USDT] = Currency.USD
        };

        private static IEnumerable<string> _formattedCurrencies => Enum.GetNames(typeof(Currency));
        private static IEnumerable<string> _rawCurrencies => Enum.GetNames(typeof(RawCurrencies));

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };

        public static string FormatPair(this string unformattedPair)
        {
            var result = HandleFormatting(unformattedPair);

            foreach (var rawCurrency in _rawCurrencies)
            {
                var index = result.IndexOf(rawCurrency, StringComparison.InvariantCultureIgnoreCase);

                if (index != -1)
                {
                    // Remove raw currency string
                    result = result.Remove(index, rawCurrency.Length);

                    // Convert string back to enum
                    Enum.TryParse(rawCurrency, true, out RawCurrencies keyValue);

                    // Replace raw currency by formatted currency substring
                    result = result.Insert(index, CurrencyDict[keyValue].ToString());
                }
            }

            return result;
        }

        // Choose handler for current format case
        private static string HandleFormatting(string unformattedPair)
        {
            switch (unformattedPair)
            {
                case var up when unformattedPair.EndsWith(Currency.USD.ToString()):
                    return FormatAsUsdPair(unformattedPair);

                case var up when unformattedPair.EndsWith(RawCurrencies.USDT.ToString()):
                    return FormatAsUsdtPair(unformattedPair);

                case var up when _formattedCurrencies.Any(fc => unformattedPair.EndsWith(fc.ToString())):
                    return FormatAsTwoWayCryptoPair(up, _formattedCurrencies.FirstOrDefault(fc => up.EndsWith(fc.ToString())).ToString());

                case var up when _rawCurrencies.Any(fc => unformattedPair.EndsWith(fc.ToString())):
                    return FormatAsTwoWayCryptoPair(up, _rawCurrencies.FirstOrDefault(fc => up.EndsWith(fc.ToString())).ToString()) ;

                default:
                    throw new FormatPatternException($"{unformattedPair} doesn't match any format pattern");
            }
        }

        // Format as pair with USD, e.g. "BTC/USD"
        private static string FormatAsUsdPair(string pair)
        {
            pair = pair.Substring(1);

            var delimIndex = pair.Length - 4;

            // Removing delimeter letter before second currency
            pair = pair.Remove(delimIndex, 1);

            // Inserting needed delimeter
            pair = pair.Insert(delimIndex, "/");

            return pair;
        }

        // Format as pair with USDT, e.g. "BTC/USDT"
        private static string FormatAsUsdtPair(string pair)
        {
            pair = pair.Insert(pair.Length - 5, "/");

            return pair;
        }

        // Format as pair of two cryptocurrencies, e.g. "BTC/ETH"
        private static string FormatAsTwoWayCryptoPair(string pair, string secondCurrency)
        {
            pair = pair.Substring(1);

            var secondCurLength = secondCurrency.Length;

            // Removing delimeter letter before second currency
            pair = pair.Remove(secondCurLength + 1, 1);

            // Inserting needed delimeter
            pair = pair.Insert(secondCurLength, "/");

            return pair;
        }
    }
}
