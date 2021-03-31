using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class ClosedTradesTabViewModel : BindableBase, ITradesReloadHandler
    {
        public ObservableCollection<ClosedPriceItemDto> TableClosedTrades { get; set; } = new ObservableCollection<ClosedPriceItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetCurrenciesForCombobox();

        #region SelectedCurrency
        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                SetProperty(ref _selectedCurrency, value);
            }
        }
        #endregion

        #region StartDate
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        #endregion

        #region StartFromFirstTrade
        private bool _startFromFirstTrade;
        public bool StartFromFirstTrade
        {
            get => _startFromFirstTrade;
            set => SetProperty(ref _startFromFirstTrade, value);
        }
        #endregion

        #region EndDate
        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }
        #endregion

        #region EndOnToLastTrade
        private bool _endOnLastTrade;
        public bool EndOnToLastTrade
        {
            get => _endOnLastTrade;
            set => SetProperty(ref _endOnLastTrade, value);
        }
        #endregion

        #region TradesAmount
        private decimal _tradesAmount;
        public decimal TradesAmount
        {
            get => _tradesAmount;
            set => SetProperty(ref _tradesAmount, value);
        }
        #endregion

        #region AvgProfitPerTrade
        private decimal _avgProfitPerTrade;
        public decimal AvgProfitPerTrade
        {
            get => _avgProfitPerTrade;
            set => SetProperty(ref _avgProfitPerTrade, value);
        }
        #endregion

        #region TradesAbsProfit
        private decimal _tradesAbsProfit;
        public decimal TradesAbsProfit
        {
            get => _tradesAbsProfit;
            set => SetProperty(ref _tradesAbsProfit, value);
        }
        #endregion

        #region TraderProfit
        private decimal _traderProfit;
        public decimal TraderProfit
        {
            get => _traderProfit;
            set => SetProperty(ref _traderProfit, value);
        }
        #endregion

        #region TradesAbsProfit
        private decimal _tradesPureAbsProfit;
        public decimal TradesPureAbsProfit
        {
            get => _tradesPureAbsProfit;
            set => SetProperty(ref _tradesPureAbsProfit, value);
        }
        #endregion

        public void OnTradesReload()
        {
            throw new NotImplementedException();
        }
    }
}
