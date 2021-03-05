using System.Windows;
using System.Windows.Controls;
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

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }
    }
}
