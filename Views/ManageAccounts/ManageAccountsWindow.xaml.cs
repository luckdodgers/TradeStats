using System;
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

            Closed += ManageAccountsWindow_Closed;
        }

        private void ManageAccountsWindow_Closed(object sender, EventArgs e)
        {
            ((IDisposable)DataContext).Dispose();
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
