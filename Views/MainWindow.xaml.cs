using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TradeStats.Extensions;
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

        private const string itemToMergeColor = "#acc4e8";
        private readonly SolidColorBrush itemToMergeBrush = itemToMergeColor.ToBrush();

        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            DataContext = container.Resolve<MainWindowViewModel>();
            ((IHandleAccountSwitch)DataContext).OnAccountSwitch();
        }

        private void SetTradesGridSelectedItemColor(SolidColorBrush brush)
        {
            var row = (DataGridRow)TradesDataGrid.ItemContainerGenerator.ContainerFromItem(TradesDataGrid.SelectedItem);
            row.Background = brush;
        }

        private void ClearTradesGridSelectedItemColor()
        {
            var row = (DataGridRow)TradesDataGrid.ItemContainerGenerator.ContainerFromItem(TradesDataGrid.SelectedItem);
            row.Background = Brushes.Transparent;
        }

        private void AddToMergeBtn_Click(object sender, RoutedEventArgs e) => SetTradesGridSelectedItemColor(itemToMergeBrush);

        private void UncheckAllBtn_Click(object sender, RoutedEventArgs e) => ClearTradesGridSelectedItemColor();

        private void MergeTradesBtn_Click(object sender, RoutedEventArgs e) => ClearTradesGridSelectedItemColor();
    }
}
