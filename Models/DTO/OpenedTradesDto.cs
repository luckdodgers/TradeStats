using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeStats.Models.DTO
{
    class OpenedTradesDto
    {
        public long Id { get; set; }
        public bool IsChecked { get; set; }
        public DateTime Datetime { get; set; }
        public string Pair { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
        public decimal Result { get; set; }
    }
}
