using System;

namespace TradeStats.Models.Domain
{
    public class Trade
    {
        protected Trade() { }

        public Trade(int accountId, DateTime datetime, TradeSide type, Currency firstCurrency, Currency secondCurrency,
            decimal price, decimal amount, decimal sum, decimal fee)
        {
            AccountId = accountId;
            Datetime = datetime;
            Side = type;
            FirstCurrency = firstCurrency;
            SecondCurrency = secondCurrency;
            Price = price;
            Amount = amount;
            Sum = sum;
            Residue = amount; // NP: Check for correctness while getting from DB
            Fee = fee;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }
        public TradeSide Side { get; }
        public Currency FirstCurrency { get; }
        public Currency SecondCurrency { get; }
        public decimal Price { get; }
        public decimal Amount { get; }
        public decimal Sum { get; private set; }
        public decimal Fee { get; private set; }
        public decimal Residue { get; private set; }
        public bool IsClosed { get; private set; }

        /// <returns>closeAmount residue</returns>
        public decimal SubstractCloseAmount(decimal closeAmount)
        {
            if (Residue >= closeAmount)
            {
                Residue -= closeAmount;
                Sum = Price * Residue;
                return 0;
            }

            var closeAmountResidue = closeAmount - Residue;

            Residue = 0;
            IsClosed = true;

            return closeAmountResidue;
        }

        public void SetResidue(decimal newResidue)
        {
            Residue = newResidue;
            Sum = Price * Residue;

            if (newResidue == 0)
                IsClosed = true;
        }

        public void SetFee(decimal newFee) => Fee = newFee;
    }
}
