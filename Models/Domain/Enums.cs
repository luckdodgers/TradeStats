namespace TradeStats.Models.Domain
{
    public enum TradeSide : short { Buy, Sell }

    public enum Position : short { Long, Short }

    public enum RawCurrencies : short
    {
        XBT
    }

    public enum Currency : short
    {
        USDT,
        USD,
        BTC,
        ETH,
        XRP,
        DOT,
        ADA,
        LINK,
        BNB
    }
}
