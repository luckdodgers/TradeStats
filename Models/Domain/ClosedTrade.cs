using System;
using System.Collections.Generic;
using System.Linq;
using TradeStats.Extensions;

namespace TradeStats.Models.Domain
{
    public class ClosedTrade
    {
        protected ClosedTrade() { }

        public ClosedTrade(int accountId, DateTime datetime, Currency firstCurrency, Currency secondCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee, Currency exchangeFeeCurrency, decimal traderFee)
        {
            AccountId = accountId;
            Datetime = datetime;
            FirstCurrency = firstCurrency;
            SecondCurrency = secondCurrency;
            BuyPrice = openPrice;
            SellPrice = closePrice;
            Amount = amount;
            ExchangeRoundFee = roundFee;
            TraderFee = traderFee;
            ExchangeFeeCurrency = exchangeFeeCurrency;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }

        // Currency has been spent in open trade
        public Currency FirstCurrency { get; }

        // Currency has been spent in close trade
        public Currency SecondCurrency { get; } 

        public decimal BuyPrice { get; }
        public decimal SellPrice { get; }
        public decimal Amount { get; }
        public decimal ExchangeRoundFee { get; }
        public Currency ExchangeFeeCurrency { get; }
        public decimal TraderFee { get; private set; }

        private decimal? _absProfit;

        public static ClosedTrade Create(OpenTrade firstTrade, OpenTrade secondTrade, MergeResultData mergeResult, decimal traderFee)
        {
            OpenTrade buyTrade = default;
            OpenTrade sellTrade = default;

            switch (firstTrade.Side)
            {
                case TradeSide.Buy:
                    buyTrade = firstTrade;
                    sellTrade = secondTrade;
                    break;

                case TradeSide.Sell:
                    buyTrade = secondTrade;
                    sellTrade = firstTrade;
                    break;
            }

            return new ClosedTrade
                (
                    accountId: sellTrade.AccountId,
                    datetime: sellTrade.Datetime,
                    firstCurrency: sellTrade.FirstCurrency,
                    secondCurrency: sellTrade.SecondCurrency,
                    openPrice: buyTrade.Price,
                    closePrice: sellTrade.Price,
                    amount: mergeResult.Amount,
                    roundFee: mergeResult.ExchangeFee,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: traderFee
                );
        }

        public static ClosedTrade Create(Currency firstCurrency, Currency secondCurrency, 
            decimal buyPrice, decimal amount, decimal sellPrice, decimal roundFee, decimal traderFee)
        {
            return new ClosedTrade
                (
                    accountId: -1,
                    datetime: DateTime.MinValue,
                    firstCurrency: firstCurrency,
                    secondCurrency: secondCurrency,
                    openPrice: buyPrice,
                    closePrice: sellPrice,
                    amount: amount,
                    roundFee: roundFee,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: traderFee
                );
        }

        public static ClosedTrade CreateWeightedAverage(IEnumerable<ClosedTrade> tradesToAggregate, decimal traderFee)
        {
            if (!tradesToAggregate.Any(t => t != null))
                return null;

            var commonDataSource = tradesToAggregate.First(t => t != null);
            var avgBuyPrice = tradesToAggregate.WeightedAverage(ct => ct.BuyPrice, ct => ct.Amount);
            var avgSellPrice = tradesToAggregate.WeightedAverage(ct => ct.SellPrice, ct => ct.Amount);
            var amountSum = tradesToAggregate.Sum(ct => ct.Amount);
            var roundFeesSum = tradesToAggregate.Sum(ct => ct.ExchangeRoundFee);

            return new ClosedTrade
                (
                    accountId: commonDataSource.AccountId,
                    datetime: tradesToAggregate.Max(t => t.Datetime),
                    firstCurrency: commonDataSource.FirstCurrency,
                    secondCurrency: commonDataSource.SecondCurrency,
                    openPrice: avgBuyPrice,
                    closePrice: avgSellPrice,
                    amount: amountSum,
                    roundFee: roundFeesSum,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: traderFee
                );
        }

        public void SetNewTraderFee(decimal newFee) => TraderFee = newFee;

        // Abs profit before subtracting trader fee
        public decimal GetAbsProfit()
        {
            _absProfit = SellPrice * Amount - BuyPrice * Amount;

            if (ExchangeFeeCurrency == Currency.USD || ExchangeFeeCurrency == Currency.USDT)
                _absProfit -= ExchangeRoundFee;

            return _absProfit.Value;
        }

        // Percentage of profit before any fee subtracting
        public decimal GetPercentageProfit() => ((SellPrice / BuyPrice) - 1m) * 100m;

        public decimal GetTraderProfit() => GetAbsProfit() * (TraderFee / 100);

        public decimal GetPureAbsProfit() => GetAbsProfit() - GetTraderProfit();

        public decimal GetOpenSum() => BuyPrice * Amount;
    }
}