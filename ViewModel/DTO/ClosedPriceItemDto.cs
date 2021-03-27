using System;
using TradeStats.Models.Domain;

namespace TradeStats.ViewModel.DTO
{
    public class ClosedPriceItemDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Currency FirstCurrency { get; set; }
        public Currency SecondCurrency { get; set; }
        public string Pair { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Amount { get; set; }
        public decimal ProfitPerTrade { get; set; }
        public decimal AbsProfit { get; set; }
        public decimal TraderProfit { get; set; }
        public decimal PureAbsProfit { get; set; }
    }
}
