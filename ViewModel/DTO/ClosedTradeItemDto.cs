using System;
using TradeStats.Models.Domain;

namespace TradeStats.ViewModel.DTO
{
    public class ClosedTradeItemDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Currency FirstCurrency { get; set; }
        public Currency SecondCurrency { get; set; }
        public string Pair { get; set; }
        public string OpenPrice { get; set; }
        public string ClosePrice { get; set; }
        public string Sum { get; set; }
        public string ProfitPerTrade { get; set; }
        public string AbsProfit { get; set; }
        public string TraderProfit { get; set; }
        public string PureAbsProfit { get; set; }
    }
}
