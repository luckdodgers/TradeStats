using System.Windows;
using System.Windows.Controls;
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

        private const string _itemToMergeColor = "#acc4e8";
        private readonly SolidColorBrush _itemToMergeBrush = _itemToMergeColor.ToBrush();
        private readonly IMainWindowViewModel _viewModel;

        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            DataContext = container.Resolve<MainWindowViewModel>();

            _viewModel = DataContext as IMainWindowViewModel;
            ((ITradesReloadHandler)DataContext).OnTradesReload();
        }

        private void ClearTradesGridItemColor()
        {
            foreach (var item in TradesDataGrid.Items)
            {
                var row = (DataGridRow)TradesDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                row.Background = Brushes.Transparent;
            }
        }

        private void AddToMergeBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedDto = (TradeMergeItemDto)TradesDataGrid.SelectedItem;
            var mergeTabValidations = _viewModel.TradesMergeTab as ITradesMergeTabValidations;

            if (mergeTabValidations.IsAddToMergePossibe(selectedDto))
            {
                // Setting color for selected and added for merge row
                var row = (DataGridRow)TradesDataGrid.ItemContainerGenerator.ContainerFromItem(TradesDataGrid.SelectedItem);
                row.Background = _itemToMergeBrush;
            }
        }

        private void UncheckAllBtn_Click(object sender, RoutedEventArgs e) => ClearTradesGridItemColor();

        private void MergeTradesBtn_Click(object sender, RoutedEventArgs e) => ClearTradesGridItemColor();
    }
}
