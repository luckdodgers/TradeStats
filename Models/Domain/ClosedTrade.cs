using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Models.Common;

namespace TradeStats.Models.Domain
{
    public class ClosedTrade
    {
        protected ClosedTrade() { }

        public ClosedTrade(int accountId, DateTime datetime, Currency buyCurrency, Currency sellCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee)
        {
            AccountId = accountId;
            Datetime = datetime;
            BuyCurrency = buyCurrency;
            SellCurrency = sellCurrency;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Amount = amount;
            RoundFee = roundFee;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }
        public Currency BuyCurrency { get; }
        public Currency SellCurrency { get; }
        public decimal OpenPrice { get; }
        public decimal ClosePrice { get; }
        public decimal Amount { get; }
        public decimal RoundFee { get; }

        public static ClosedTrade Create(Trade openTrade, Trade closeTrade)
        {
            return new ClosedTrade
                (
                    accountId: closeTrade.AccountId,
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
