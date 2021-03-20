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
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class TradesMergeTabViewModel : BindableBase, IHandleAccountSwitch, IDisposable
    {
        public ObservableCollection<TradeMergeItemDto> TradeMergeItems { get; set; } = new ObservableCollection<TradeMergeItemDto>();

        #region Commands
        public ICommand MergeCommand { get; private set; }
        public ICommand UncheckAllCommand { get; private set; }
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

        #region SelectedTradesCounter
        private string _selectedTradesCounter = "0/2";
        public string SelectedTradesCounter
        {
            get => _selectedTradesCounter;
            set => SetProperty(ref _selectedTradesCounter, value);
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

            MergeCommand = new DelegateCommand(async () => await Merge(), CanMerge).ObservesProperty(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(async () => await UncheckAll());
            AddToMergeCommand = new DelegateCommand<object>(AddToMerge);
        }

        public async void OnAccountSwitch()
        {
            if (_accountCache.CurrentAccount != null)
            {
                var accountTrades = _context.OpenTrades.Where(t => t.AccountId == _accountCache.CurrentAccount.Id);

                LastTradeDate = accountTrades.Any() ?
                    accountTrades.Max(t => t.Datetime).ToString("dd'.'MM'.'yyyy")
                    : "No trades so far";
            }

            else
            {
                LastTradeDate = string.Empty;
            }

            await ReloadTableData();
        }

        private async Task ReloadTableData()
        {
            if (_accountCache.CurrentAccount == null)
            {
                TradeMergeItems.Clear();
                return;
            }

            var openedTrades = await _context.OpenTrades
                .Where(t => t.AccountId == _accountCache.CurrentAccount.Id && !t.IsClosed)
                .ToListAsync();

            TradeMergeItems.Clear();
            TradeMergeItems.AddRange(_mapper.Map<List<OpenTrade>, List<TradeMergeItemDto>>(openedTrades));
        }

        private void AddToMerge(object tradeToAdd)
        {
            var tradeDto = tradeToAdd as TradeMergeItemDto;

            _selectedTradesToMerge.Add(tradeDto);
            SelectedTradesCounter = $"{_selectedTradesToMerge.Count}/2";

            IsAddBtnEnabled = _selectedTradesToMerge.Count < 2;
        }

        private async Task Merge()
        {
            
        }

        private bool CanMerge()
        {
            return true;
        }

        private async Task UncheckAll()
        {

        }

        public void Dispose()
        {
            _accountCache.CacheUpdated -= OnAccountSwitch;
        }
    }
}
