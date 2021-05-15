using AutoMapper;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;
using TradeStats.ViewModel.Interfaces;
using TradeStats.ViewModel.MainWindow.Tabs;
using TradeStats.Views;
using Unity;

namespace TradeStats.ViewModel.MainWindow
{
    using static WindowExtensions;

    class MainWindowViewModel : BindableBase, IMainWindowViewModel, ITradesReloadHandler
    {
        private readonly TradesMergeTabViewModel _tradesMergeTab;
        public TradesMergeTabViewModel TradesMergeTab => _tradesMergeTab;

        private readonly ClosedTradesTabViewModel _closedTradesTab;
        public ClosedTradesTabViewModel ClosedTradesTab => _closedTradesTab;

        private readonly IUnityContainer _container;
        private readonly IExternalDataManager _dataManager;
        private readonly IOpenTradesLoader _openTradesLoader;
        private readonly ICachedData<Account> _curCachedAccount;
        private readonly IConfigurationProvider _configProvider;
        private readonly ICurrentAccountTradeContext _curAccountContext;

        private event Action _TradesImported;

        #region Commands
        public ICommand OpenManageAccountsWindowCommand { get; private set; }
        public ICommand OpenImportWindowCommand { get; private set; }
        public ICommand OpenExportWindowCommand { get; private set; }
        #endregion

        #region IsHistoryImportMenuItemEnabled
        private bool _isHistoryImportMenuItemEnabled;
        public bool IsHistoryImportMenuItemEnabled
        {
            get => _isHistoryImportMenuItemEnabled;
            set => SetProperty(ref _isHistoryImportMenuItemEnabled, value);
        }
        #endregion   

        #region IsHistoryExportMenuItemEnabled
        private bool _isHistoryExportMenuItemEnabled = true;
        public bool IsHistoryExportMenuItemEnabled
        {
            get => _isHistoryExportMenuItemEnabled;
            set => SetProperty(ref _isHistoryExportMenuItemEnabled, value);
        }
        #endregion 

        public MainWindowViewModel(IUnityContainer container, IExternalDataManager dataManager, IOpenTradesLoader openTradesLoader,
            ICachedData<Account> curCachedAccount, IConfigurationProvider configProvider, ICurrentAccountTradeContext curAccountContext, IMapper mapper)
        {
            _container = container;
            _dataManager = dataManager;
            _openTradesLoader = openTradesLoader;
            _curCachedAccount = curCachedAccount;
            _configProvider = configProvider;
            _curAccountContext = curAccountContext;

            _tradesMergeTab = new TradesMergeTabViewModel(_curCachedAccount, _configProvider, _curAccountContext);
            _closedTradesTab = new ClosedTradesTabViewModel(_curAccountContext, mapper);

            _curCachedAccount.CacheUpdated += OnTradesReload;
            _TradesImported += TradesMergeTab.OnTradesReload;
            _TradesImported += ClosedTradesTab.OnTradesReload;

            OpenManageAccountsWindowCommand = new DelegateCommand(OpenManageAccountsWindow);
            OpenImportWindowCommand = new DelegateCommand(async () => await ImportTradeHistory());
            OpenExportWindowCommand = new DelegateCommand(async () => await ExportClosedTrades());
        }

        public void OnTradesReload()
        {
            TradesMergeTab.OnTradesReload();

            IsHistoryImportMenuItemEnabled = _curCachedAccount.CurrentAccount != null;
        }

        private void OpenManageAccountsWindow()
        {
            if (IsWindowOpened<ManageAccountsWindow>(out var window))
                window.Activate();

            else new ManageAccountsWindow(_container).ShowDialog();
        }

        private async Task ImportTradeHistory()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.RestoreDirectory = true;          

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var loadedTrades = await _dataManager.ImportTradesHistory(filePath);
                
                await _openTradesLoader.UpdateOpenTrades(loadedTrades);

                _TradesImported?.Invoke();
            }
        }

        private async Task ExportClosedTrades()
        {
            string ConstructFilename()
            {
                var startDate = ClosedTradesTab.StartFromFirstTrade ?
                    ((IEnumerable<ClosedTrade>)ClosedTradesTab.CurrentClosedTrades).Min(t => t.Datetime).ToShortDateString() : ClosedTradesTab.StartDate.ToShortDateString();
                var endDate = ClosedTradesTab.EndOnLastTrade ? 
                    ((IEnumerable<ClosedTrade>)ClosedTradesTab.CurrentClosedTrades).Max(t => t.Datetime).ToShortDateString() : ClosedTradesTab.EndDate.ToShortDateString();
                var accountName = _curCachedAccount.CurrentAccount.AccountName;

                return $"{accountName}_from_{startDate.Replace('.', '_')}_to_{endDate.Replace('.', '_')}";
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = ConstructFilename();

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                await _dataManager.ExportClosedTrades(ClosedTradesTab.CurrentClosedTrades.OrderBy(t => t.Datetime), saveFileDialog.FileName);
            }
        }
    }
}
