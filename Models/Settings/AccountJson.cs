using System;
using System.Collections.Generic;

namespace TradeStats.Models.Settings
{
    class AccountsData
    {
        public List<AccountJson> Accounts { get; set; }
    }

    public class AccountJson
    {
        public string Name { get; set; }
        public Exchanges Exchange { get; set; }
        public List<FeeData> FeeDataList { get; set; }
    }

    public class FeeData
    {
        public DateTime ActiveFrom { get; set; }
        public int Fee { get; set; }
    }
}