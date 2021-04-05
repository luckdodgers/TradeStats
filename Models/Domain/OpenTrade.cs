using System;

namespace TradeStats.Models.Domain
{
    public class OpenTrade
    {
        protected OpenTrade() { }

        public OpenTrade(int accountId, DateTime datetime, TradeSide type, Currency firstCurrency, Currency secondCurrency,
            decimal price, decimal amount, decimal sum, Currency feeCurency, decimal fee)
        {
            AccountId = accountId;
            Datetime = datetime;
            Side = type;
            FirstCurrency = firstCurrency;
            SecondCurrency = secondCurrency;
            Price = price;
            Amount = amount;
            Sum = sum;
            FeeCurrency = feeCurency;
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
        public Currency FeeCurrency { get; private set; }
        public decimal Fee { get; private set; }
        public bool IsClosed { get; private set; }

        // Returns merge amount
        public MergeResultData MergeWith(OpenTrade secondTrade)
        {
            MergeResultData mergeResultData;

            if (Amount > secondTrade.Amount)
            {
                var amountResidue = Amount - secondTrade.Amount;
                var mergeFee = Fee * (secondTrade.Amount / Amount) + secondTrade.Fee;
                var feeResidue = Fee - mergeFee;

                mergeResultData = new MergeResultData(secondTrade.Amount, mergeFee);

                SetResidue(amountResidue, feeResidue);
                secondTrade.SetResidue(decimal.Zero, decimal.Zero);
            }

            else
            {
                var secondTradeAmountResidue = secondTrade.Amount - Amount;
                var mergeFee = secondTrade.Fee * (Amount / secondTrade.Amount) + Fee;
                var secondTradeFeeResidue = mergeFee;

                mergeResultData = new MergeResultData(Amount, mergeFee);

                secondTrade.SetResidue(secondTradeAmountResidue, secondTradeFeeResidue);
                SetResidue(decimal.Zero, decimal.Zero);
            }

            return mergeResultData;
        }

        public MergeResultData GetPotentialMergeData(OpenTrade secondTrade)
        {
            MergeResultData mergeResultData;

            if (Amount > secondTrade.Amount)
            {
                var mergeFee = Fee * (secondTrade.Amount / Amount) + secondTrade.Fee;
                mergeResultData = new MergeResultData(secondTrade.Amount, mergeFee);
            }

            else
            {
                var mergeFee = secondTrade.Fee * (Amount / secondTrade.Amount) + Fee;
                mergeResultData = new MergeResultData(Amount, mergeFee);
            }

            return mergeResultData;
        }

        public void SetResidue(decimal newAmount, decimal newFee)
        {
            Amount = newAmount;
            Fee = newFee;
            Sum = Price * Amount;

            if (newAmount == decimal.Zero)
                IsClosed = true;
        }
    }
}
