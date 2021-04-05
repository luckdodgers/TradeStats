namespace TradeStats.Models.Domain
{
    public struct MergeResultData
    {
        public MergeResultData(decimal amount, decimal exchangeFee)
        {
            Amount = amount;
            ExchangeFee = exchangeFee;
        }

        public decimal Amount { get; }
        public decimal ExchangeFee { get; }
    }
}
