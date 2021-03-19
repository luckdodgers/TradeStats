using System;
using TradeReportsConverter.Exceptions;
using TradeStats.Exceptions;
using TradeStats.Models.Domain;

namespace TradeStats.Extensions
{
    public static class EnumParseExtension
    {
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
            var formattedValue = rawValue.FormatPair();
            var pairArray = formattedValue.Split('/');

            var isFirstParseSuccessful = Enum.TryParse(typeof(Currency), pairArray[0], out var firstCurrency);
            var isSecondParseSuccessful = Enum.TryParse(typeof(Currency), pairArray[1], out var secondeCurrency);

            if (!isFirstParseSuccessful || !isSecondParseSuccessful)
                throw new FormatPatternException($"Can't format value=\"{rawValue}\" into {nameof(Currency)} enum");

            return ((Currency)firstCurrency, (Currency)secondeCurrency);
        }
    }
}
