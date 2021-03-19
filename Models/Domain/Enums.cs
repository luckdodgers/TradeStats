using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStats.Models.Domain
{
    public enum TradeSide : short { Buy, Sell }

    public enum RawCurrencies : short
    {
        USDT,
        XBT
    }

    public enum Currency : short
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
