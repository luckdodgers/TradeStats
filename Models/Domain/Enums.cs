namespace TradeStats.Models.Domain
{
    public enum TradeSide : short { Buy, Sell }

    public enum RawCurrencies : short
    {
        //USDT,
        XBT
    }

    public enum Currency : short
    {
        USD,
        USDT,
        BTC,
        ETH,
        XRP,
        DOT,
        NEO,
        ADA,
        LINK
    }
}
