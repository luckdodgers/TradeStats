using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Exceptions;
using TradeStats.Models.Domain;
using TradeStats.ViewModel.DTO;

namespace TradeStats.Extensions
{
    public static class TradeMergeDtoExtensions
    {
        public static bool HasCommonCurrencyWith(this TradeMergeItemDto fisrtTradeMergeItem, TradeMergeItemDto secondTradeMergeItem)
        {
            var currencies = new List<Currency>()
            {
                fisrtTradeMergeItem.FirstCurrency,
                fisrtTradeMergeItem.SecondCurrency,
                secondTradeMergeItem.FirstCurrency,
                secondTradeMergeItem.SecondCurrency
            };

            return currencies.Distinct().Count() < 4;
        } 

        public static bool CanBeThirdCurrencyIn(this TradeMergeItemDto tradeMergeItem, List<TradeMergeItemDto> alreadyAddedTrades)
        {
            if (alreadyAddedTrades.Count != 2)
                throw new SelectedTradesWrongAmountException($"Added for merge trades amount should be 2. Actual amount is {alreadyAddedTrades.Count}.");

            var currencies = alreadyAddedTrades.GetCurrencies();

            var duplicateCurrency = currencies.GroupBy(c => c)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .Single();

            currencies.RemoveAll(c => c == duplicateCurrency);

            return currencies.TrueForAll(c => c == tradeMergeItem.FirstCurrency || c == tradeMergeItem.SecondCurrency);
        }

        public static List<Currency> GetCurrencies(this List<TradeMergeItemDto> trades)
        {
            var currencies = new List<Currency>(4);

            foreach (var trade in trades)
            {
                currencies.Add(trade.FirstCurrency);
                currencies.Add(trade.SecondCurrency);
            }

            return currencies;
        }
    }
}
