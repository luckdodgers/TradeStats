using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class ClosedTradesTabViewModel : BindableBase, ITradesReloadHandler
    {
        public ObservableCollection<ClosedTradeItemDto> TableClosedTrades { get; set; } = new ObservableCollection<ClosedTradeItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetStringCurrenciesForCombobox();

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

        #region EndOnLastTrade
        private bool _endOnLastTrade;
        public bool EndOnLastTrade
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

        private readonly ICurrentAccountTradeContext _context;
        private readonly IConfigurationProvider _configProvider;

        public ICommand LoadTradesCommand { get; private set; }

        public ClosedTradesTabViewModel(ICurrentAccountTradeContext context, IConfigurationProvider configProvider)
        {
            _context = context;
            _configProvider = configProvider;

            LoadTradesCommand = new DelegateCommand(async () => await LoadTrades());

            SelectedCurrency = CurrenciesList[0];
        }

        public async void OnTradesReload()
        {
            
        }

        private async Task LoadTrades()
        {
            var closedTrades = _context.CurrentAccountClosedTrades
                .Where(t => t.FirstCurrency == Enum.Parse<Currency>(SelectedCurrency));

            closedTrades = StartFromFirstTrade ? closedTrades : closedTrades.Where(t => t.Datetime >= StartDate);
            closedTrades = EndOnLastTrade ? closedTrades : closedTrades.Where(t => t.Datetime <= EndDate);

            var closedTradesList = await closedTrades.ProjectToListAsync<ClosedTradeItemDto>(_configProvider);

            TableClosedTrades.SetWithDataGridSorting(closedTradesList);
        }
    }
}
