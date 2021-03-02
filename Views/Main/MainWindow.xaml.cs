using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TradeStats.ViewModel.MainWindow;
using Unity;

namespace TradeStats.Views.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUnityContainer _container;

        public MainWindow(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            DataContext = container.Resolve<MainWindowViewModel>();
        }
    }
}
