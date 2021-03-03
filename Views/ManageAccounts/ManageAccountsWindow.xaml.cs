using System.Windows;
using TradeStats.ViewModel.ManageAccounts;
using Unity;

namespace TradeStats.Views.ManageAccounts
{
    /// <summary>
    /// Interaction logic for ManageAccountsWindow.xaml
    /// </summary>
    public partial class ManageAccountsWindow : Window
    {
        private readonly IUnityContainer _container;

        public ManageAccountsWindow(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            DataContext = container.Resolve<ManageAccountsViewModel>();
        }
    }
}
