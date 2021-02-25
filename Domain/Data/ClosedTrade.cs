using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Domain.Common;

namespace TradeStats.Domain.Data
{
    class ClosedTrade
    {
        public ClosedTrade (DateTime datetime, Currency buyCurrency, Currency sellCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee)
        {
            Datetime = datetime;
            BuyCurrency = buyCurrency;
            SellCurrency = sellCurrency;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Amount = amount;
            RoundFee = roundFee;
        }

        public long Id { get; }
        public DateTime Datetime { get; }
        public Currency BuyCurrency { get; }
        public Currency SellCurrency { get; }
        public decimal OpenPrice { get; }
        public decimal ClosePrice { get; }
        public decimal Amount { get; }
        public decimal RoundFee { get; }

        public static ClosedTrade Create (Trade openTrade, Trade closeTrade)
        {
            return new ClosedTrade
                (
                    datetime: closeTrade.Datetime,
                    buyCurrency: closeTrade.SecondCurrency,
                    sellCurrency: closeTrade.FirstCurrency,
                    openPrice: openTrade.Price,
                    closePrice: closeTrade.Price,
                    amount: openTrade.Amount,
                    roundFee: openTrade.Fee + closeTrade.Fee
                );
        }
    }
}
