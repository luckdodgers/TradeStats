using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.ViewModel.MainWindow.Tabs;
using TradeStats.Views.ManageAccounts;
using Unity;

namespace TradeStats.ViewModel.MainWindow
{
    using static WindowExtensions;

    class MainWindowViewModel : BindableBase
    {
        private readonly IUnityContainer _container;

        private TradesMergeTabViewModel _tradesMergeTab = new TradesMergeTabViewModel();
        public TradesMergeTabViewModel TradesMergeTab => _tradesMergeTab;

        public ICommand OpenManageAccountsWindowCommand { get; private set; }

        public MainWindowViewModel(IUnityContainer container)
        {
            _container = container;

            OpenManageAccountsWindowCommand = new DelegateCommand(OpenManageAccountsWindow);
        }

        private void OpenManageAccountsWindow()
        {
            if (IsWindowOpened<ManageAccountsWindow>(out var window))
                window.Activate();

            else new ManageAccountsWindow(_container).ShowDialog();
        }  
    }
}
