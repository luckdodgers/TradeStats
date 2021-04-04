using System;

namespace TradeStats.Models.Domain
{
    public class ClosedTrade
    {
        protected ClosedTrade() { }

        public ClosedTrade(int accountId, DateTime datetime, Position position, Currency firstCurrency, Currency secondCurrency,
            decimal openPrice, decimal closePrice, decimal amount, decimal roundFee, Currency exchangeFeeCurrency, decimal traderFee)
        {
            AccountId = accountId;
            Datetime = datetime;
            Position = position;
            FirstCurrency = firstCurrency;
            SecondCurrency = secondCurrency;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Amount = amount;
            ExchangeRoundFee = roundFee;
            TraderFee = traderFee;
            ExchangeFeeCurrency = exchangeFeeCurrency;
        }

        public long Id { get; }
        public int AccountId { get; }
        public DateTime Datetime { get; }
        public Position Position { get; }

        // Currency has been spent in open trade
        public Currency FirstCurrency { get; }

        // Currency has been spent in close trade
        public Currency SecondCurrency { get; } 

        public decimal OpenPrice { get; }
        public decimal ClosePrice { get; }
        public decimal Amount { get; }
        public decimal ExchangeRoundFee { get; }
        public Currency ExchangeFeeCurrency { get; }
        public decimal TraderFee { get; private set; }

        private decimal? _absProfit;

        public static ClosedTrade Create(OpenTrade openTrade, OpenTrade closeTrade, decimal tradeAmount, decimal currentTraderFee)
        {
            Position tradePosition = default;

            switch (closeTrade.Side)
            {
                case TradeSide.Sell:
                    tradePosition = Position.Long;
                    break;

                case TradeSide.Buy:
                    tradePosition = Position.Short;
                    break;
            }

            return new ClosedTrade
                (
                    accountId: closeTrade.AccountId,
                    datetime: closeTrade.Datetime,
                    position: tradePosition,
                    firstCurrency: closeTrade.FirstCurrency,
                    secondCurrency: closeTrade.SecondCurrency,
                    openPrice: openTrade.Price,
                    closePrice: closeTrade.Price,
                    amount: tradeAmount,
                    roundFee: openTrade.Fee + closeTrade.Fee,
                    exchangeFeeCurrency: Currency.USD,
                    traderFee: currentTraderFee
                );
        }

        //public static ClosedTrade Create(OpenTrade openTrade, OpenTrade middleTrade, OpenTrade closeTrade, decimal tradeAmount, decimal currentTraderFee)
        //{
        //    return new ClosedTrade
        //        (
        //            accountId: closeTrade.AccountId,
        //            datetime: closeTrade.Datetime,
        //            position: Position.Long,
        //            firstCurrency: openTrade.FirstCurrency,
        //            secondCurrency: closeTrade.SecondCurrency,
        //            openPrice: openTrade.Price,
        //            closePrice: 
        //        );
        //}

        public void SetNewTraderFee(decimal newFee) => TraderFee = newFee;

        // Abs profit before subtracting trader fee
        public decimal GetAbsProfit()
        {
            if (_absProfit != null)
                return _absProfit.Value;

            switch (Position)
            {
                case Position.Long:
                    _absProfit = ClosePrice * Amount - OpenPrice * Amount;
                    break;

                case Position.Short:
                    _absProfit = OpenPrice * Amount - ClosePrice * Amount;
                    break;
            }

            if (ExchangeFeeCurrency == Currency.USD || ExchangeFeeCurrency == Currency.USDT)
                _absProfit -= ExchangeRoundFee;

            return _absProfit.Value;
        }

        // Abs profit before subtracting trader fee
        public decimal GetPercentageProfit()
        {
            decimal exchangeFeeToSubtract = decimal.Zero;

            if (ExchangeFeeCurrency == Currency.USD || ExchangeFeeCurrency == Currency.USDT)
                exchangeFeeToSubtract -= ExchangeRoundFee;

            switch (Position)
            {
                case Position.Long:
                    return ((ClosePrice * Amount) / (OpenPrice * Amount - exchangeFeeToSubtract) - 1m) * 100m;

                case Position.Short:
                    return ((OpenPrice * Amount) / (ClosePrice * Amount - exchangeFeeToSubtract) - 1m) * 100m;

                default:
                    throw new NotImplementedException();
            }
        }

        public decimal GetTraderProfit() => GetAbsProfit() * TraderFee / 100;

        public decimal GetPureAbsProfit() => GetAbsProfit() - GetTraderProfit();

        public decimal GetOpenSum() => OpenPrice * Amount;
    }
}
