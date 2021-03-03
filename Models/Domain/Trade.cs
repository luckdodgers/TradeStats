using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Models.Common;

namespace TradeStats.Models.Domain
{
    class Trade
    {
        public Trade(DateTime datetime, TradeSide type, Currency firstCurrency, Currency secondCurrency,
            decimal price, decimal amount, decimal fee)
        {
            Datetime = datetime;
            Side = type;
            FirstCurrency = firstCurrency;
            SecondCurrency = secondCurrency;
            Price = price;
            Amount = amount;
            Residue = amount; // Check for correctness while getting from DB
            Fee = fee;
        }

        public long Id { get; }
        public DateTime Datetime { get; }
        public TradeSide Side { get; }
        public Currency FirstCurrency { get; }
        public Currency SecondCurrency { get; }
        public decimal Price { get; }
        public decimal Amount { get; }
        public decimal Fee { get; }
        public decimal Residue { get; private set; }
        public bool IsClosed { get; private set; }

        /// <returns>closeAmount residue</returns>
        public decimal SubstractCloseAmount(decimal closeAmount)
        {
            if (Residue >= closeAmount)
            {
                Residue -= closeAmount;
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

            if (newResidue == 0)
                IsClosed = true;
        }
    }
}
