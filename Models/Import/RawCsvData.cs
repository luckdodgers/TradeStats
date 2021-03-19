using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeStats.Models.Import
{
    class RawCsvData
    {
        public string Pair { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal Fee { get; set; }
        public decimal Vol { get; set; }
    }
}
