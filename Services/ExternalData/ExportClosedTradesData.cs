namespace TradeStats.Services.ExternalData
{
    class ExportClosedTradesData
    {
        public string Date { get; set; }
        public string Pair { get; set; }
        public string OpenPrice { get; set; }
        public string Sum { get; set; }
        public string ClosePrice { get; set; }
        public string ProfitPerTrade { get; set; }
        public string AbsProfit { get; set; }
        public string PureAbsProfit { get; set; }
    }
}
