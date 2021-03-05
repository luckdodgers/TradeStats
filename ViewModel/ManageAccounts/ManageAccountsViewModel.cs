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
        public ObservableCollection<string> EditAccountExchange { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> NewAccountExchange { get; set; } = new ObservableCollection<string>();

        #region Commands
        public ICommand SwitchAccountCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand RemoveAccountCommand { get; private set; }
        public ICommand SaveEditedAccountCommand { get; private set; }
        public ICommand AddNewAccountCommand { get; private set; }
        #endregion

        #region EditAccountName
        private string _editAccountName;
        public string EditAccountName
        {
            get => _editAccountName;
            set => SetProperty(ref _editAccountName, value);
        }
        #endregion

        #region EditAccountTraderFee
        private decimal _editAccountTraderFee;
        public decimal EditAccountTraderFee
        {
            get => _editAccountTraderFee;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Trader fee cannot be negative or greater than 100");

                SetProperty(ref _editAccountTraderFee, value);
            }
        }
        #endregion

        #region NewFeeStartDate 
        private DateTime _newFeeStartDate;
        public DateTime NewFeeStartDate
        {
            get => _newFeeStartDate;
            set => SetProperty(ref _newFeeStartDate, value);
        }
        #endregion

        #region IsNewFeeStartsFromNextTrade
        private bool _isNewFeeStartsFromNextTrade;
        public bool IsNewFeeStartsFromNextTrade
        {
            get => _isNewFeeStartsFromNextTrade;
            set => SetProperty(ref _isNewFeeStartsFromNextTrade, value);
        }
        #endregion

        #region NewAccountName
        private string _newAccountName;
        public string NewAccountName
        {
            get => _newAccountName;
            set => SetProperty(ref _newAccountName, value);
        }
        #endregion

        #region NewAccountTraderFee
        private decimal _newAccountTraderFee;
        public decimal NewAccountTraderFee
        {
            get => _newAccountTraderFee;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Trader fee cannot be negative or greater than 100");

                SetProperty(ref _newAccountTraderFee, value);
            }
        }
        #endregion
    }
}
