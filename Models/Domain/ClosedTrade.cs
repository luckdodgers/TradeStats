using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStats.Models.Domain
{
    public class ClosedTrade
    {
        protected ClosedTrade() { }

        public ClosedTrade(int accountId, DateTime datetime, Currency buyCurrency, Currency sellCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee, decimal traderFee)
        {
            AccountId = accountId;
            Datetime = datetime;
            BuyCurrency = buyCurrency;
            SellCurrency = sellCurrency;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Amount = amount;
            ExchangeRoundFee = roundFee;
            TraderFee = traderFee;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }
        public Currency BuyCurrency { get; }
        public Currency SellCurrency { get; }
        public decimal OpenPrice { get; }
        public decimal ClosePrice { get; }
        public decimal Amount { get; }
        public decimal ExchangeRoundFee { get; }
        public decimal TraderFee { get; private set; }

        public static ClosedTrade Create(OpenTrade openTrade, OpenTrade closeTrade, decimal tradeAmount, decimal currentTraderFee)
        {
            return new ClosedTrade
                (
                    accountId: closeTrade.AccountId,
                    datetime: closeTrade.Datetime,
                    buyCurrency: closeTrade.SecondCurrency,
                    sellCurrency: closeTrade.FirstCurrency,
                    openPrice: openTrade.Price,
                    closePrice: closeTrade.Price,
                    amount: tradeAmount,
                    roundFee: openTrade.Fee + closeTrade.Fee,
                    traderFee: currentTraderFee
                );
        }

        public void SetNewTraderFee(decimal newFee) => TraderFee = newFee;
    }
}
