using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeStats.Views.Main
{
    public class TradeMergeItemDto
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Date { get; set; }
        public string Pair { get; set; }
        public string Side { get; set; }
        public string Price { get; set; }
        public string Sum { get; set; }
        public string Result { get; set; }
    }
}
