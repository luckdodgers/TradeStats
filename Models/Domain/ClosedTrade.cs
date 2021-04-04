using System;

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

        public static ClosedTrade Create(OpenTrade firstTrade, OpenTrade secondTrade, decimal tradeAmount, decimal currentTraderFee)
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
                    amount: tradeAmount,
                    roundFee: buyTrade.Fee + sellTrade.Fee,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: currentTraderFee
                );
        }

        public void SetNewTraderFee(decimal newFee) => TraderFee = newFee;

        // Abs profit before subtracting trader fee
        public decimal GetAbsProfit()
        {
            if (_absProfit != null)
                return _absProfit.Value;

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
