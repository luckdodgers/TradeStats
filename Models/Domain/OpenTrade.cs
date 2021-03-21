using System;
using TradeStats.Services.Interfaces;

namespace TradeStats.Models.Domain
{
    public class OpenTrade
    {
        protected OpenTrade() { }

        public OpenTrade(int accountId, DateTime datetime, TradeSide type, Currency firstCurrency, Currency secondCurrency,
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
            Fee = fee;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }
        public TradeSide Side { get; }
        public Currency FirstCurrency { get; }
        public Currency SecondCurrency { get; }
        public decimal Price { get; }
        public decimal Amount { get; private set; }
        public decimal Sum { get; private set; }
        public decimal Fee { get; private set; }
        public bool IsClosed { get; private set; }

        /// <returns>closeAmount residue</returns>
        public decimal SubstractCloseAmount(decimal closeAmount)
        {
            if (Amount > closeAmount)
            {
                Amount -= closeAmount;
                Sum = Price * Amount;
                return 0;
            }

            var closeAmountResidue = closeAmount - Amount;

            Amount = 0;
            Sum = 0;
            IsClosed = true;

            return closeAmountResidue;
        }

        public void SetResidue(decimal newResidue)
        {
            Amount = newResidue;
            Sum = Price * Amount;

            if (newResidue == 0)
                IsClosed = true;
        }

        public void SetFee(decimal newFee) => Fee = newFee;

        public bool IsEqualByValue(OpenTrade comparingTrade)
        {
            return Datetime == comparingTrade.Datetime
                && Side == comparingTrade.Side
                && FirstCurrency == comparingTrade.FirstCurrency
                && SecondCurrency == comparingTrade.SecondCurrency
                && Price == comparingTrade.Price
                && Amount == comparingTrade.Amount
                && Sum == comparingTrade.Sum;        
        }
    }
}
