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
using TradeStats.Exceptions;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;
using TradeReportsConverter.Extensions;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    public class TradesMergeTabViewModel : BindableBase, ITradesMergeTabValidations, ITradesReloadHandler
    {
        public ObservableCollection<TradeMergeItemDto> TableOpenTrades { get; set; } = new ObservableCollection<TradeMergeItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetCurrenciesForCombobox();

        #region Commands
        public ICommand MergeCommand { get; private set; }
        public ICommand UncheckAllCommand { get; private set; }
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

        private readonly ICachedData<Account> _accountCache;
        private readonly IConfigurationProvider _configProvider;
        private readonly ICurrentAccountTradeContext _context;

        private IReadOnlyList<TradeMergeItemDto> _allOpenTradeDtoList = new List<TradeMergeItemDto>();
        private List<TradeMergeItemDto> _selectedTradesToMerge = new();
        private ClosedTrade _closingTrade = null;

        public ICommand AddToMergeCommand { get; private set; }

        public TradesMergeTabViewModel(ICachedData<Account> accountCache, IConfigurationProvider configProvider, ICurrentAccountTradeContext curAccountContext)
        {
            _accountCache = accountCache;
            _configProvider = configProvider;
            _context = curAccountContext;

            MergeCommand = new DelegateCommand(async () => await Merge()).ObservesCanExecute(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(UncheckAll);
            AddToMergeCommand = new DelegateCommand<object>(async (obj) => await AddToMerge(obj));

            SelectedCurrency = CurrenciesList[0];
        }

        public bool IsAddToMergePossibe(TradeMergeItemDto tradeDto)
        {
            //if (SelectedCurrency == CurrencyOrderRule.AnyCurrencyString)
            //{
            //    switch (_selectedTradesToMerge.Count)
            //    {
            //        case 0:
            //            return true;

            //        case 1:
            //            var alreadySelected = _selectedTradesToMerge.Peek();
            //            return tradeDto.HasCommonCurrencyWith(alreadySelected);

            //        case 2:
            //            return tradeDto.CanBeThirdCurrencyIn(_selectedTradesToMerge);

            //        default:
            //            throw new SelectedTradesWrongAmountException($"Added for merge trades amount should be 2. Actual amount is {_selectedTradesToMerge.Count}.");
            //    }
            //}

            //else
            
            return !_selectedTradesToMerge.Contains(tradeDto) && !_selectedTradesToMerge.Any(t => t.Side == tradeDto.Side);
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
            if (selectedCurrency == CurrencyOrderRule.AnyCurrencyString)
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

                var closedTradeAmount = openTrade.GetPotentialMergeAmount(closeTrade);
                _closingTrade = ClosedTrade.Create(openTrade, closeTrade, closedTradeAmount, _accountCache.CurrentAccount.Fee);

                UpdateTradeStats();
            }
        }

        private async Task Merge()
        {
            IsMergeBtnEnabled = false;

            var tradesToMerge = await _context.CurrentAccountOpenTrades
                .Where(ot => _selectedTradesToMerge.Select(mt => mt.Id).Contains(ot.Id))
                .ToListAsync();

            var closedTradeAmount = tradesToMerge[0].MergeWith(tradesToMerge[1]);
            _context.TradesContext.OpenTrades.RemoveRange(tradesToMerge.Where(ttm => ttm.IsClosed));

            await _context.TradesContext.ClosedTrades.AddAsync(_closingTrade);
            await _context.SaveChangesAsync();

            IsAddBtnEnabled = true;
            _closingTrade = null;

            OnTradesReload();
            UpdateTradeStats();
        }

        private void UncheckAll()
        {
            IsAddBtnEnabled = true;
            IsMergeBtnEnabled = false;
            _selectedTradesToMerge.Clear();

            UpdateTradeStats();
        }

        private void UpdateTradeStats()
        {
            if (_closingTrade != null)
            {
                ProfitPerTradeText = _closingTrade.GetPercentageProfit().TwoDigitsAfterDot();
                AbsProfitText = _closingTrade.GetAbsProfit().TwoDigitsAfterDot();
            }
            
            else
            {
                ProfitPerTradeText = string.Empty;
                AbsProfitText = string.Empty;
            }
        }
    }
}
