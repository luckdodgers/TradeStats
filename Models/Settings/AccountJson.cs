using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TradeStats.Models.Settings
{
    class AccountsData
    {
        public string CurrentAccount { get; set; }
        public List<AccountJson> Accounts { get; set; } = new List<AccountJson>();

        public bool IsAccountAlreadyExist(string accountName) => Accounts.Any(a => a.Name == accountName);
    }

    public class AccountJson
    {
        [JsonConstructor]
        public AccountJson(string name, Exchanges exchange, List<FeeData> feeDataList)
        {
            Name = name;
            Exchange = exchange;
            FeeDataList = feeDataList;
        }

        public string Name { get; set; }
        public Exchanges Exchange { get; set; }
        public List<FeeData> FeeDataList { get; set; }
    }

    public class FeeData
    {
        [JsonConstructor]
        public FeeData(DateTime activeFrom, decimal fee)
        {
            ActiveFrom = activeFrom;
            Fee = fee;
        }

        public DateTime ActiveFrom { get; set; }
        public decimal Fee { get; set; }
    }
}