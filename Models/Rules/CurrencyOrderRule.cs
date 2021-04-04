using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Models.Rules
{
    public static class CurrencyOrderRule
    {
        public static readonly string AnyCurrencyString = "- All -";

        public static List<string> GetStringCurrenciesForCombobox(this IEnumerable<Currency> currencies)
        {
            var filteredCollection = currencies.Except(new List<Currency>() { Currency.USDT, Currency.USD }).ToList();
            var orderedList = new List<Currency>() { Currency.BTC, Currency.ETH };

            orderedList.AddRange(filteredCollection.Except(orderedList).OrderBy(cur => cur.ToString()));

            var resultList = orderedList.Select(cur => cur.ToString()).ToList();
            resultList.Insert(0, AnyCurrencyString);

            return resultList;
        }
    }
}
