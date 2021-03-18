using System;
using TradeStats.Models.Common;

namespace TradeStats.Models.ViewModel
{
    class OpenedTradesDto
    {
        public long Id { get; set; }
        public bool IsChecked { get; set; }
        public DateTime Datetime { get; set; }
        public string Pair { get; set; }
        public TradeSide Side { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
        public decimal Result { get; set; }
    }
}
