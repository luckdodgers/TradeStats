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

    class MainWindowViewModel : BindableBase, IMainWindowViewModel, IHandleAccountSwitch, IDisposable
    {
        private readonly IUnityContainer _container;
        private readonly ICsvImport<OpenTrade> _dataSource;
        private readonly IOpenTradesLoader _openTradesLoader;
        private readonly ICachedData<Account> _curCachedAccount;
        private readonly ITradesContext _context;
        private readonly IMapper _mapper;

        private readonly TradesMergeTabViewModel _tradesMergeTab;
        public TradesMergeTabViewModel TradesMergeTab => _tradesMergeTab;

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
            ICachedData<Account> curCachedAccount, ITradesContext context, IMapper mapper)
        {
            _container = container;
            _dataSource = dataSource;
            _openTradesLoader = openTradesLoader;
            _curCachedAccount = curCachedAccount;
            _context = context;
            _mapper = mapper;

            _tradesMergeTab = new TradesMergeTabViewModel(_curCachedAccount, _context, _mapper);
            _curCachedAccount.CacheUpdated += OnAccountSwitch;

            OpenManageAccountsWindowCommand = new DelegateCommand(OpenManageAccountsWindow);
            OpenImportWindowCommand = new DelegateCommand(async () => await OpenImportWindow());
            
        }

        public async void OnAccountSwitch()
        {
            TradesMergeTab.OnAccountSwitch();

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
            IEnumerable<OpenTrade> loadedTrades = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.RestoreDirectory = true;          

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                loadedTrades = await _dataSource.LoadData(filePath);
                //try
                //{
                    
                //}

                //catch (System.IO.IOException)
                //{
                //    MessageBox.Show("Please, close importing file and try again", "Error. File opened.");
                //    return;
                //}
                
                await _openTradesLoader.UpdateOpenTrades(loadedTrades);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
