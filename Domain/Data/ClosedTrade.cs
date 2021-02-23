using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Domain.Common;

namespace TradeStats.Domain.Data
{
    class ClosedTrade
    {
        public ClosedTrade(DateTime datetime, Currency openCurrency, Currency closeCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee)
        {
            Datetime = datetime;
            OpenCurrency = openCurrency;
            CloseCurrency = closeCurrency;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Amount = amount;
            RoundFee = roundFee;
        }

        public long Id { get; }
        public DateTime Datetime { get; }
        public Currency OpenCurrency { get; }
        public Currency CloseCurrency { get; }
        public decimal OpenPrice { get; }
        public decimal ClosePrice { get; }
        public decimal Amount { get; }
        public decimal RoundFee { get; }
    }
}
