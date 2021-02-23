using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStats.Domain.Common
{
    public enum TradeSide { Buy, Sell }

    public enum RawCurrencies
    {
        USDT,
        XBT
    }

    public enum Currency
    {
        USD,
        BTC,
        ETH,
        XRP,
        DOT,
        NEO,
        ADA,
        LINK
    }
}
