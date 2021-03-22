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
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    public class TradesMergeTabViewModel : BindableBase, ITradesMergeTabValidations, ITradesReloadHandler, IDisposable
    {
        public ObservableCollection<TradeMergeItemDto> TableOpenTrades { get; set; } = new ObservableCollection<TradeMergeItemDto>();
        public IReadOnlyList<string> CurrenciesList { get; set; } = Enum.GetValues<Currency>().GetCurrenciesForCombobox();

        private IReadOnlyList<TradeMergeItemDto> _allOpenTradeDtoList = new List<TradeMergeItemDto>();
        private IReadOnlyList<OpenTrade> _allOpenTrades = new List<OpenTrade>();

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

        #region MergingTradesCounter
        private string _mergingTradesCounter = "0/2";
        public string MergingTradesCounter
        {
            get => _mergingTradesCounter;
            set => SetProperty(ref _mergingTradesCounter, value);
        }
        #endregion

        private readonly ICachedData<Account> _accountCache;
        private readonly ITradesContext _context;
        private readonly IMapper _mapper;

        private List<TradeMergeItemDto> _selectedTradesToMerge = new();

        public ICommand AddToMergeCommand { get; private set; }

        public TradesMergeTabViewModel(ICachedData<Account> accountCache, ITradesContext context, IMapper mapper)
        {
            _accountCache = accountCache;
            _context = context;
            _mapper = mapper;

            MergeCommand = new DelegateCommand(async () => await Merge()).ObservesCanExecute(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(async () => await UncheckAll());
            AddToMergeCommand = new DelegateCommand<object>(AddToMerge);

            SelectedCurrency = CurrenciesList[0];
        }

        public bool IsAddToMergePossibe(TradeMergeItemDto tradeDto) => !_selectedTradesToMerge.Contains(tradeDto) && !_selectedTradesToMerge.Any(t => t.Side == tradeDto.Side);

        public async void OnTradesReload()
        {
            if (_accountCache.CurrentAccount != null)
            {
                var accountOpenTrades = _context.OpenTrades
                    .Where(t => t.AccountId == _accountCache.CurrentAccount.Id);

                var accountClosedTradesDates = _context.ClosedTrades
                    .Where(t => t.AccountId == _accountCache.CurrentAccount.Id)
                    .Select(t => t.Datetime);

                LastTradeDate = accountOpenTrades.Any() ?
                    accountOpenTrades.Select(t => t.Datetime).Concat(accountClosedTradesDates).Max().ToString("dd'.'MM'.'yyyy")
                    : "No trades so far";
            }

            else
            {
                LastTradeDate = string.Empty;
            }

            await ReloadTableOnAccountSwitch();
        }

        private async Task ReloadTableOnAccountSwitch()
        {
            if (_accountCache.CurrentAccount == null)
            {
                TableOpenTrades.Clear();
                return;
            }

            _allOpenTrades = await _context.OpenTrades
                .AsNoTracking()
                .Where(t => t.AccountId == _accountCache.CurrentAccount.Id && !t.IsClosed)
                .ToListAsync();

            _allOpenTradeDtoList = _mapper.Map<IReadOnlyList<OpenTrade>, IReadOnlyList<TradeMergeItemDto>>(_allOpenTrades);

            TableOpenTrades.SetWithDataGridSorting(_allOpenTradeDtoList);
        }

        private void OnCurrencySelect(string selectedCurrency)
        {
            if (selectedCurrency == CurrencyOrderRule.AnyCurrencyString)
                TableOpenTrades.SetWithDataGridSorting(_allOpenTradeDtoList);

            else
            {
                var selectedCurrencyEnum = Enum.Parse<Currency>(selectedCurrency);
                var filteredOpenTrades = _allOpenTrades.Where(t => t.FirstCurrency == selectedCurrencyEnum);
                var filteredOpenTradesDto = _allOpenTradeDtoList.Where(t => filteredOpenTrades.Select(ft => ft.Id).Any(id => id == t.Id));

                TableOpenTrades.SetWithDataGridSorting(filteredOpenTradesDto);            
            }
        }

        private void AddToMerge(object tradeToAdd)
        {
            var tradeDto = tradeToAdd as TradeMergeItemDto;

            if (!IsAddToMergePossibe(tradeDto))
                return;

            _selectedTradesToMerge.Add(tradeDto);
            MergingTradesCounter = $"{_selectedTradesToMerge.Count}/2";

            IsAddBtnEnabled = _selectedTradesToMerge.Count < 2;
        }

        private async Task Merge()
        {
            
        }

        private async Task UncheckAll()
        {
            
        }

        public void Dispose()
        {
            _accountCache.CacheUpdated -= OnTradesReload;
        }
    }
}
