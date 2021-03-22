using System;

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
        public decimal Fee { get; }
        public bool IsClosed { get; private set; }

        // Returns merge amount
        public decimal MergeWith(OpenTrade secondTrade)
        {
            decimal mergeAmount;

            if (Amount > secondTrade.Amount)
            {
                mergeAmount = secondTrade.Amount;

                Amount -= secondTrade.Amount;
                secondTrade.SetResidue(decimal.Zero);
                Sum = Price * Amount;
            }

            else
            {
                mergeAmount = Amount;

                var secondTradeResidue = secondTrade.Amount - Amount;
                secondTrade.SetResidue(secondTradeResidue);
                SetResidue(decimal.Zero);
            }

            return mergeAmount;
        }

        public void SetResidue(decimal newResidue)
        {
            Amount = newResidue;
            Sum = Price * Amount;

            if (newResidue == decimal.Zero)
                IsClosed = true;
        }

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
