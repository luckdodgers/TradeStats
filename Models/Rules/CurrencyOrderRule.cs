using System.Collections.Generic;
using System.Linq;
using TradeStats.Models.Common;
using TradeStats.Models.Domain;

namespace TradeStats.Models.Rules
{
    public static class CurrencyOrderRule
    {
        public static List<string> GetStringCurrenciesForCombobox(this IEnumerable<Currency> currencies, bool includeAllValue = true)
        {
            var filteredCollection = currencies.Except(new List<Currency>() { Currency.USDT, Currency.USD }).ToList();
            var orderedList = new List<Currency>() { Currency.BTC, Currency.ETH };

            orderedList.AddRange(filteredCollection.Except(orderedList).OrderBy(cur => cur.ToString()));

            var resultList = orderedList.Select(cur => cur.ToString()).ToList();

            if (includeAllValue)
                resultList.Insert(0, Constants.AnyCurrencyString);

            return resultList;
        }
    }
}
