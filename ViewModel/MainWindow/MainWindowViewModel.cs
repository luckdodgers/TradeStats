using AutoMapper;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly ICsvImport<OpenTrade> _dataSource;
        private readonly IOpenTradesLoader _openTradesLoader;
        private readonly ICachedData<Account> _curCachedAccount;
        private readonly IConfigurationProvider _configProvider;
        private readonly ICurrentAccountTradeContext _curAccountContext;

        private event Action _TradesImported;

        public ICommand OpenManageAccountsWindowCommand { get; private set; }
        public ICommand OpenImportWindowCommand { get; private set; }

        #region IsAccountImportMenuItemEnabled
        private bool _isAccountImportMenuItemEnabled;
        public bool IsAccountImportMenuItemEnabled
        {
            get => _isAccountImportMenuItemEnabled;
            set => SetProperty(ref _isAccountImportMenuItemEnabled, value);
        }
        #endregion   

        public MainWindowViewModel(IUnityContainer container, ICsvImport<OpenTrade> dataSource, IOpenTradesLoader openTradesLoader,
            ICachedData<Account> curCachedAccount, IConfigurationProvider configProvider, ICurrentAccountTradeContext curAccountContext)
        {
            _container = container;
            _dataSource = dataSource;
            _openTradesLoader = openTradesLoader;
            _curCachedAccount = curCachedAccount;
            _configProvider = configProvider;
            _curAccountContext = curAccountContext;

            _tradesMergeTab = new TradesMergeTabViewModel(_curCachedAccount, _configProvider, _curAccountContext);
            _closedTradesTab = new ClosedTradesTabViewModel();

            _curCachedAccount.CacheUpdated += OnTradesReload;
            _TradesImported += TradesMergeTab.OnTradesReload;
            _TradesImported += ClosedTradesTab.OnTradesReload;

            OpenManageAccountsWindowCommand = new DelegateCommand(OpenManageAccountsWindow);
            OpenImportWindowCommand = new DelegateCommand(async () => await OpenImportWindow());
        }

        public void OnTradesReload()
        {
            TradesMergeTab.OnTradesReload();

            IsAccountImportMenuItemEnabled = _curCachedAccount.CurrentAccount != null;
        }

        private void OpenManageAccountsWindow()
        {
            if (IsWindowOpened<ManageAccountsWindow>(out var window))
                window.Activate();

            else new ManageAccountsWindow(_container).ShowDialog();
        }

        private async Task OpenImportWindow()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.RestoreDirectory = true;          

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var loadedTrades = await _dataSource.LoadData(filePath);
                
                await _openTradesLoader.UpdateOpenTrades(loadedTrades);

                _TradesImported?.Invoke();
            }
        }
    }
}
