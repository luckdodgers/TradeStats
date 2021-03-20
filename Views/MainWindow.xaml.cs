using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradeStats.ViewModel.DTO;
using TradeStats.ViewModel.Interfaces;
using TradeStats.ViewModel.MainWindow;
using Unity;

namespace TradeStats.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUnityContainer _container;

        private long CurrentSelectedId;

        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            DataContext = container.Resolve<MainWindowViewModel>();
            ((IHandleAccountSwitch)DataContext).OnAccountSwitch();
        }

        //private void TradesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedId = ((TradeMergeItemDto)((DataGrid)e.Source).SelectedItem).Id;
        //    ((IHandleTradeSelection)DataContext).OnTradeSelected(CurrentSelectedId);
        //}
    }
}
