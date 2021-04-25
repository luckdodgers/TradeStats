using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeReportsConverter.Extensions;
using TradeStats.Extensions;
using TradeStats.Models.Common;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    public class TradesMergeTabViewModel : BindableBase, ITradesMergeTabValidations, ITradesReloadHandler
    {
        public ObservableCollection<TradeMergeItemDto> TableOpenTrades { get; } = new ObservableCollection<TradeMergeItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetStringCurrenciesForCombobox(includeAllValue: false);

        #region Commands
        public ICommand MergeCommand { get; private set; }
        public ICommand UncheckAllCommand { get; private set; }
        public ICommand AddToMergeCommand { get; private set; }
        public ICommand MergeAllCommand { get; private set; }
        #endregion

        #region SelectedCurrency
        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                SetProperty(ref _selectedCurrency, value);
                OnCurrencySelect(_selectedCurrency);
            } 
        }
        #endregion

        #region IsMergeBtnEnabled
        private bool _isMergeBtnEnabled;
        public bool IsMergeBtnEnabled
        {
            get => _isMergeBtnEnabled;
            set => SetProperty(ref _isMergeBtnEnabled, value);
        }
        #endregion

        #region IsAddBtnEnabled
        private bool _isAddBtnEnabled = true;
        public bool IsAddBtnEnabled
        {
            get => _isAddBtnEnabled;
            set => SetProperty(ref _isAddBtnEnabled, value);
        }
        #endregion

        #region IsMergeAllEnabled
        private bool _isMergeAllEnabled = true;
        public bool IsMergeAllEnabled
        {
            get => _isMergeAllEnabled;
            set => SetProperty(ref _isMergeAllEnabled, value);
        }
        #endregion

        #region LastTradeDate
        private string _lastTradeDate;
        public string LastTradeDate
        {
            get => _lastTradeDate;
            set => SetProperty(ref _lastTradeDate, value);
        }
        #endregion

        #region ProfitPerTradeText
        private string _profitPerTradeText;
        public string ProfitPerTradeText
        {
            get => _profitPerTradeText;
            set => SetProperty(ref _profitPerTradeText, value);
        }
        #endregion

        #region AbsProfitText
        private string _absProfitText;
        public string AbsProfitText
        {
            get => _absProfitText;
            set => SetProperty(ref _absProfitText, value);
        }
        #endregion

        #region PairToMerge
        private string _pairToMerge;
        public string PairToMerge
        {
            get => _pairToMerge;
            set => SetProperty(ref _pairToMerge, value);
        }
        #endregion

        #region SideToMerge
        private string _sideToMerge;
        public string SideToMerge
        {
            get => _sideToMerge;
            set => SetProperty(ref _sideToMerge, value);
        }
        #endregion

        #region PriceToMerge
        private string _priceToMerge;
        public string PriceToMerge
        {
            get => _priceToMerge;
            set => SetProperty(ref _priceToMerge, value);
        }
        #endregion

        #region SumToMerge
        private string _sumToMerge;
        public string SumToMerge
        {
            get => _sumToMerge;
            set => SetProperty(ref _sumToMerge, value);
        }
        #endregion

        private readonly ICachedData<Account> _accountCache;
        private readonly IConfigurationProvider _configProvider;
        private readonly ICurrentAccountTradeContext _context;

        private IReadOnlyList<TradeMergeItemDto> _allOpenTradeDtoList = new List<TradeMergeItemDto>();
        private readonly List<TradeMergeItemDto> _selectedTradesToMerge = new();
        private ClosedTrade _closingTrade = null;

        public TradesMergeTabViewModel(ICachedData<Account> accountCache, IConfigurationProvider configProvider, ICurrentAccountTradeContext curAccountContext)
        {
            _accountCache = accountCache;
            _configProvider = configProvider;
            _context = curAccountContext;

            InitCommands();

            SelectedCurrency = CurrenciesList[0];
        }

        private void InitCommands()
        {
            MergeCommand = new DelegateCommand(async () => await Merge()).ObservesCanExecute(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(UncheckAll);
            AddToMergeCommand = new DelegateCommand<object>(async (obj) => await AddToMerge(obj));
            MergeAllCommand = new DelegateCommand(async () => await AutoMergeAll()).ObservesProperty(() => IsMergeAllEnabled);
        }

        public bool IsAddToMergePossibe(TradeMergeItemDto tradeDtoToAdd)
        {
            /// Checkout <see cref="TradeMergeDtoExtensions"/> class for implementing 3-way merge
             
            bool IsDtoToAddContainsSamePair()
            {
                var usdCurrencies = new Currency[] { Currency.USD, Currency.USDT };

                if (usdCurrencies.Contains(_selectedTradesToMerge[0].SecondCurrency))
                {
                    return _selectedTradesToMerge[0].FirstCurrency == tradeDtoToAdd.FirstCurrency
                        && usdCurrencies.Contains(tradeDtoToAdd.SecondCurrency);
                }

                return _selectedTradesToMerge[0].Pair == tradeDtoToAdd.Pair;
            }
            
            switch (_selectedTradesToMerge.Count)
            {
                case 0:
                    return true;

                case 1:
                   
                    return IsDtoToAddContainsSamePair() // Trying to add same pair as already added
                        && !_selectedTradesToMerge.Contains(tradeDtoToAdd) // Not trying to add same trade as already added
                        && !_selectedTradesToMerge.Any(t => t.Side == tradeDtoToAdd.Side); // Trying to add trade with reverse side

                default:
                    return false;
            }
        }

        public async void OnTradesReload()
        {
            if (_accountCache.CurrentAccount != null)
            {
                var accountClosedTradesDates = _context.CurrentAccountClosedTrades.Select(t => t.Datetime);

                LastTradeDate = _context.CurrentAccountOpenTrades.Any() ?
                    _context.CurrentAccountOpenTrades.Select(t => t.Datetime).Concat(accountClosedTradesDates).Max().ToString("dd'.'MM'.'yyyy")
                    : "No trades so far";
            }

            else
            {
                LastTradeDate = string.Empty;
            }

            await ReloadTableOnAccountSwitch();

            _selectedTradesToMerge.Clear();
            OnCurrencySelect(SelectedCurrency);
        }

        private async Task ReloadTableOnAccountSwitch()
        {
            if (_accountCache.CurrentAccount == null)
            {
                TableOpenTrades.Clear();
                return;
            }

            _allOpenTradeDtoList = await _context.CurrentAccountOpenTrades
                .AsNoTracking()
                .Where(t => !t.IsClosed)
                .ProjectToListAsync<TradeMergeItemDto>(_configProvider);             

            TableOpenTrades.SetWithDataGridSorting(_allOpenTradeDtoList);
        }

        private void OnCurrencySelect(string selectedCurrency)
        {
            if (selectedCurrency == Constants.AnyCurrencyString)
                TableOpenTrades.SetWithDataGridSorting(_allOpenTradeDtoList);

            else
            {
                var filteredOpenTradesDto = _allOpenTradeDtoList.Where(t => t.FirstCurrency == Enum.Parse<Currency>(selectedCurrency));
                TableOpenTrades.SetWithDataGridSorting(filteredOpenTradesDto);            
            }
        }

        private async Task AddToMerge(object tradeToAdd)
        {
            var tradeDto = tradeToAdd as TradeMergeItemDto;

            if (!IsAddToMergePossibe(tradeDto))
                return;

            _selectedTradesToMerge.Add(tradeDto);

            IsAddBtnEnabled = _selectedTradesToMerge.Count < 2;
            IsMergeBtnEnabled = _selectedTradesToMerge.Count == 2;

            if (_selectedTradesToMerge.Count == 2)
            {
                var openTrade = await _context.CurrentAccountOpenTrades.FirstAsync(ot => ot.Id == _selectedTradesToMerge[0].Id);
                var closeTrade = await _context.CurrentAccountOpenTrades.FirstAsync(ot => ot.Id == _selectedTradesToMerge[1].Id);

                var mergeData = openTrade.GetPotentialMergeData(closeTrade);
                _closingTrade = ClosedTrade.Create(openTrade, closeTrade, mergeData, _accountCache.CurrentAccount.Fee);

                UpdateTradeStats();
            }

            UpdateSelectedToMergeTradeData();
        }

        private async Task Merge()
        {
            IsMergeBtnEnabled = false;

            var tradesToMerge = await _context.CurrentAccountOpenTrades
                .Where(ot => _selectedTradesToMerge.Select(mt => mt.Id).Contains(ot.Id))
                .ToListAsync();

            tradesToMerge[0].MergeWith(tradesToMerge[1]);
            _context.TradesContext.OpenTrades.RemoveRange(tradesToMerge.Where(ttm => ttm.IsClosed));

            await _context.TradesContext.ClosedTrades.AddAsync(_closingTrade);
            await _context.SaveChangesAsync();

            IsAddBtnEnabled = true;
            _closingTrade = null;

            OnTradesReload();
            UpdateTradeStats();
            UpdateSelectedToMergeTradeData();
        }

        private async Task AutoMergeAll()
        {
            var selectedCurrency = Enum.Parse<Currency>(SelectedCurrency);

            var openTrades = await _context.CurrentAccountOpenTrades
                .AsNoTrackingWithIdentityResolution()
                .Where(ot => ot.FirstCurrency == selectedCurrency
                && (ot.SecondCurrency == Currency.USD || ot.SecondCurrency == Currency.USDT)
                && _allOpenTradeDtoList.Select(dto => dto.Id).Contains(ot.Id))
                .OrderBy(ot => ot.Side)
                .OrderBy(ot => ot.Price)
                .ToListAsync();

            var closedOpenTrades = new List<OpenTrade>(20);
            var closedTrades = new List<ClosedTrade>(10);

            void ProcessIfClosed(OpenTrade trade)
            {
                if (trade.IsClosed)
                {
                    closedOpenTrades.Add(trade);
                    openTrades.Remove(trade);
                }
            }

            while (true)
            {
                var buyTrade = openTrades.FirstOrDefault(t => t.Side == TradeSide.Buy && !t.IsClosed);
                var sellTrade = openTrades.FirstOrDefault(t => t.Side == TradeSide.Sell && !t.IsClosed);

                if (buyTrade != null && sellTrade != null)
                {
                    var mergeData = buyTrade.MergeWith(sellTrade);
                    var closedTrade = ClosedTrade.Create(buyTrade, sellTrade, mergeData, _accountCache.CurrentAccount.Fee);

                    closedTrades.Add(closedTrade);

                    ProcessIfClosed(buyTrade);
                    ProcessIfClosed(sellTrade);

                    continue;
                }

                break;
            }

            AggregateSmallTrades(closedTrades);

            _context.TradesContext.OpenTrades.RemoveRange(closedOpenTrades);
            await _context.TradesContext.ClosedTrades.AddRangeAsync(closedTrades);
            await _context.SaveChangesAsync();

            OnTradesReload();
        }

        private void AggregateSmallTrades(List<ClosedTrade> closedTrades)
        {
            var smallTrades = closedTrades
                .Where(t => t.GetOpenSum() <= Constants.SmallTradeAveragingThreshold);

            if (!smallTrades.Any())
                return;

            var smallTradesGroups = smallTrades.GroupBy(t => t.FirstCurrency);

            foreach (var group in smallTradesGroups)
            {
                var aggregatedClosedTrade = ClosedTrade.CreateWeightedAverage(group.ToList(), _accountCache.CurrentAccount.Fee);

                var nonSmallTrades = closedTrades.Where(t => t.FirstCurrency == group.Key && t.GetOpenSum() > Constants.SmallTradeAveragingThreshold);
                var minAmountTrade = nonSmallTrades?.FirstOrDefault(t => t.Amount == nonSmallTrades.Min(t => t.Amount));

                if (minAmountTrade != null)
                {
                    closedTrades.Add(minAmountTrade.MergeWith(aggregatedClosedTrade, _accountCache.CurrentAccount.Fee));
                    closedTrades.Remove(minAmountTrade);
                }

                else closedTrades.Add(aggregatedClosedTrade);
            }

            closedTrades.RemoveAll(t => smallTrades.Contains(t));
        }

        private void UncheckAll()
        {
            IsAddBtnEnabled = true;
            IsMergeBtnEnabled = false;
            _selectedTradesToMerge.Clear();

            UpdateTradeStats();
            UpdateSelectedToMergeTradeData();
        }

        private void UpdateTradeStats()
        {
            if (_closingTrade != null)
            {
                ProfitPerTradeText = _closingTrade.GetPercentageProfit().TwoDigitsAfterDotRoundUp();
                AbsProfitText = _closingTrade.GetAbsProfit().TwoDigitsAfterDotRoundUp();
            }
            
            else
            {
                ProfitPerTradeText = string.Empty;
                AbsProfitText = string.Empty;
            }
        }

        private void UpdateSelectedToMergeTradeData()
        {
            if (_selectedTradesToMerge.Count == 1)
            {
                PairToMerge = _selectedTradesToMerge[0].Pair;
                SideToMerge = _selectedTradesToMerge[0].Side.ToString();
                PriceToMerge = _selectedTradesToMerge[0].Price;
                SumToMerge = _selectedTradesToMerge[0].Sum;
            }

            else
            {
                PairToMerge = string.Empty;
                SideToMerge = string.Empty;
                PriceToMerge = string.Empty;
                SumToMerge = string.Empty;
            }
        }
    }
}
