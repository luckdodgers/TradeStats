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

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class TradesMergeTabViewModel : BindableBase, IDisposable
    {
        public TradesMergeTabViewModel()
        {
            MergeCommand = new DelegateCommand(async () => await Merge(), CanMerge).ObservesProperty(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(async () => await UncheckAll());
        }

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

        private readonly ICachedData<Account> _accountCache;
        private readonly ITradesContext _context;
        private readonly IMapper _mapper;

        public TradesMergeTabViewModel(ICachedData<Account> accountCache)
        {
            _accountCache = accountCache;
            _accountCache.CacheUpdated += OnCurrentAccountChange;
        }

        private async void OnCurrentAccountChange() => await ReloadData();

        private async Task ReloadData()
        {
            var openedTrades = await _context.Trades
                .Where(t => t.AccountId == _accountCache.CurrentAccount.Id && !t.IsClosed)
                .ToListAsync();

            TradeMergeItems.Clear();
            TradeMergeItems.AddRange(_mapper.Map<List<OpenTrade>, List<TradeMergeItemDto>>(openedTrades));
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
            _accountCache.CacheUpdated -= OnCurrentAccountChange;
        }
    }
}
