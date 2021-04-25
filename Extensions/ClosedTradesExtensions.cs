using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Extensions
{
    public static class ClosedTradesExtensions
    {
        public static ClosedTrade MergeWith(this ClosedTrade existingClosedTrade, ClosedTrade tradeToMerge, decimal traderFee)
        {
            var pair = new ClosedTrade[2] { existingClosedTrade, tradeToMerge };

            var avgWeightBuyPrice = pair.WeightedAverage(ct => ct.BuyPrice, ct => ct.Amount);
            var avgWeightSellPrice = pair.WeightedAverage(ct => ct.SellPrice, ct => ct.Amount);
            var amountSum = pair.Sum(ct => ct.Amount);
            var roundFeeSum = pair.Sum(ct => ct.ExchangeRoundFee);

            return new ClosedTrade
                (
                    accountId: existingClosedTrade.AccountId,
                    datetime: existingClosedTrade.Datetime,
                    firstCurrency: existingClosedTrade.FirstCurrency,
                    secondCurrency: existingClosedTrade.SecondCurrency,
                    openPrice: avgWeightBuyPrice,
                    closePrice: avgWeightSellPrice,
                    amount: amountSum,
                    roundFee: roundFeeSum,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: traderFee
                );
        }
    }
}
