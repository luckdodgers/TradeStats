using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Models.Rules
{
    static class ImportDataFilteringRule
    {
        private static readonly Dictionary<Currency, Currency> _pairsToRemove = new()
        {
            [Currency.USD] = Currency.USDT,
        };

        public static List<OpenTrade> RemoveFiatExchanges(this IEnumerable<OpenTrade> trades)
        {
            var tradesToRemove = trades.Where(t => _pairsToRemove.Keys.Any(key => key == t.FirstCurrency) && _pairsToRemove[t.FirstCurrency] == t.SecondCurrency);
            var tradesList = trades.ToList();
            tradesList.RemoveAll(t => tradesToRemove.Contains(t));

            return tradesList;
        }
    }
}
