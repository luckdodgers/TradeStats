using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TradeStats.Models.Domain;

namespace TradeStats.Models.Common.Comparers
{
    class OpenTradeValueComparer : EqualityComparer<OpenTrade>
    {
        public override bool Equals(OpenTrade firstTrade, OpenTrade secondTrade)
        {
            return firstTrade.Datetime == secondTrade.Datetime
                && firstTrade.Side == secondTrade.Side
                && firstTrade.FirstCurrency == secondTrade.FirstCurrency
                && firstTrade.SecondCurrency == secondTrade.SecondCurrency
                && firstTrade.Price == secondTrade.Price
                && firstTrade.Amount == secondTrade.Amount
                && firstTrade.Sum == secondTrade.Sum;
        }

        public override int GetHashCode([DisallowNull] OpenTrade trade)
        {
            return trade.Datetime.GetHashCode() 
                ^ trade.Price.GetHashCode() 
                ^ trade.Amount.GetHashCode();
        }
    }
}
