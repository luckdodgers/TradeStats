using TradeStats.Models.Settings;

namespace TradeStats.Models.Domain
{
    public class Account
    {
        protected Account() { }

        public Account(string accountName, Exchanges exchange, decimal fee)
        {
            AccountName = accountName;
            Exchange = exchange;
            Fee = fee;
        }

        public int Id { get; }
        public bool IsActive { get; private set; }
        public string AccountName { get; private set; }
        public Exchanges Exchange { get; private set; }
        public decimal Fee { get; private set; }

        public void RenameAccount(string newName) => AccountName = newName;
        public void SwitchExchange(Exchanges newExchange) => Exchange = newExchange;
        public void ChangeFee(decimal newFee) => Fee = newFee;
        public void SetActive() => IsActive = true;
        public void SetInactive() => IsActive = false;
    }
}
