using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Extensions;
using TradeStats.Models.Common;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class ClosedTradesTabViewModel : BindableBase, ITradesReloadHandler
    {
        public ObservableCollection<ClosedTradeItemDto> TableClosedTrades { get; } = new ObservableCollection<ClosedTradeItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetStringCurrenciesForCombobox();

        public ICommand LoadTradesCommand { get; private set; }

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
        private DateTime _startDate = DateTime.Today.AddMonths(-1); 
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
        private DateTime _endDate = DateTime.Today;
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
        private readonly IMapper _mapper;

        private List<ClosedTrade> _currentClosedTrades = new List<ClosedTrade>();

        public ClosedTradesTabViewModel(ICurrentAccountTradeContext context, IConfigurationProvider configProvider, IMapper mapper)
        {
            _context = context;
            _configProvider = configProvider;
            _mapper = mapper;

            LoadTradesCommand = new DelegateCommand(async () => await LoadTrades());

            SelectedCurrency = CurrenciesList[0];
            TableClosedTrades.CollectionChanged += UpdateClosedTradesStats;
        }

        public async void OnTradesReload()
        {
            
        }

        private async Task LoadTrades()
        {
            var closedTrades = _context.CurrentAccountClosedTrades;

            if (SelectedCurrency != Constants.AnyCurrencyString)
                closedTrades = closedTrades.Where(t => t.FirstCurrency == Enum.Parse<Currency>(SelectedCurrency));

            var startDate = StartFromFirstTrade ? DateTime.MinValue : StartDate;
            var endDate = EndOnLastTrade ? DateTime.MaxValue : EndDate;

            _currentClosedTrades = await closedTrades
                .Where(t => t.Datetime >= startDate && t.Datetime <= endDate)
                .ToListAsync();

            var closedTradesDtoList = _mapper.Map<IEnumerable<ClosedTrade>, IEnumerable<ClosedTradeItemDto>>(_currentClosedTrades);
            TableClosedTrades.SetWithDataGridSorting(closedTradesDtoList);
        }

        private void UpdateClosedTradesStats(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TableClosedTrades.Count > 0)
            {
                TradesAmount = TableClosedTrades.Count;
                AvgProfitPerTrade = Math.Round(_currentClosedTrades.WeightedAverage(t => t.Amount, t => t.GetPercentageProfit()), 2);
                TraderProfit = Math.Round(_currentClosedTrades.Sum(t => t.GetAbsProfit()));
                TradesPureAbsProfit = Math.Round(_currentClosedTrades.Sum(t => t.GetPureAbsProfit()));
                TradesAbsProfit = TraderProfit + TradesPureAbsProfit;
            } 
            
            else
            {
                TradesAmount = 0;
                AvgProfitPerTrade = 0;
                TraderProfit = 0;
                TradesPureAbsProfit = 0;
                TradesAbsProfit = 0;
            }
        }
    }
}
