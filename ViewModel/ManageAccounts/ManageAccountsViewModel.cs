using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradeStats.ViewModel.ManageAccounts
{
    class ManageAccountsViewModel : BindableBase
    {
        public ObservableCollection<string> ExistingAccounts { get; set; } = new ObservableCollection<string>();

        public ICommand SwitchAccountCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand RemoveAccountCommand { get; private set; }
    }
}
