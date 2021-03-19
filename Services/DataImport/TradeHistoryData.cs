using System;
using System.Collections.Generic;
using System.Text;

namespace TradeReportsConverter.Models
{
    public class TradeHistoryData
    {
        public string Date { get; set; }
        public string Pair { get; set; }
        public string Price { get; set; }
        public string Sum { get; set; }
        public string Result { get; set; }
        public string Action { get; set; }
    }
}
