using System;
using TradeStats.Models.Domain;

namespace TradeStats.ViewModel.DTO
{
    public class TradeMergeItemDto
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public DateTime Date { get; set; }
        public string Pair { get; set; }
        public TradeSide Side { get; set; }
        public string Price { get; set; }
        public string Sum { get; set; }
        public string Amount { get; set; }
    }
}
