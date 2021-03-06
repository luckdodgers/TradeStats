using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Services.Validations;

namespace TradeStats.ViewModel.ManageAccounts
{
    class ManageAccountsViewModel : BaseViewModel
    {
        public ObservableCollection<string> ExistingAccounts { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> EditAccountExchange { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> NewAccountExchange { get; set; } = new ObservableCollection<string>();

        #region Commands
        public ICommand SwitchAccountCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }
        public ICommand SaveEditedAccountCommand { get; private set; }
        public ICommand AddNewAccountCommand { get; private set; }
        #endregion

        #region IsSaveEditedAccountBtnEnabled
        private bool _isSaveEditedAccountBtnEnabled;
        public bool IsSaveEditedAccountBtnEnabled
        {
            get => _isSaveEditedAccountBtnEnabled;
            set => SetProperty(ref _isSaveEditedAccountBtnEnabled, value);
        }
        #endregion

        private bool _isAddNewAccountBtnEnabled;
        public bool IsAddNewAccountBtnEnabled
        {
            get => _isAddNewAccountBtnEnabled;
            set => SetProperty(ref _isAddNewAccountBtnEnabled, value);
        }

        #region SelectedAccount
        private string _selectedAccount;
        public string SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }
        #endregion

        #region EditAccountName
        private string _editAccountName;
        public string EditAccountName
        {
            get => _editAccountName;
            set => SetProperty(ref _editAccountName, value);
        }
        #endregion

        #region SelectedEditAccountExchange
        private string _selectedEditAccountExchange;
        public string SelectedEditAccountExchange
        {
            get => _selectedEditAccountExchange;
            set => SetProperty(ref _selectedEditAccountExchange, value);
        }
        #endregion

        #region EditAccountTraderFee
        private string _editAccountTraderFeeString;
        public string EditAccountTraderFeeString
        {
            get => _editAccountTraderFeeString;
            set
            {
                ValidateProperty(value);
                SetProperty(ref _editAccountTraderFeeString, value);
            }
        }
        #endregion

        #region NewFeeStartDate 
        private DateTime _newFeeStartDate = DateTime.Now;
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
        private string _newAccountTraderFeeString;

        public string NewAccountTraderFeeString
        {
            get => _newAccountTraderFeeString;
            set
            {
                ValidateProperty(value);
                SetProperty(ref _newAccountTraderFeeString, value);
            }
        }
        #endregion

        #region Validations
        private readonly PositivePercentValidation _traderFeeValidation = new();
        private readonly NotEmptyStringValidation _accountNameValidation = new();
        #endregion

        public ManageAccountsViewModel()
        {
            AddValidators(nameof(EditAccountName), _accountNameValidation);
            AddValidators(nameof(EditAccountTraderFeeString), _traderFeeValidation);

            AddValidators(nameof(NewAccountName), _accountNameValidation);
            AddValidators(nameof(NewAccountTraderFeeString), _traderFeeValidation);

            SwitchAccountCommand = new DelegateCommand(async () => await SwitchAccount());
            EditAccountCommand = new DelegateCommand(async () => await EditAccount());
            DeleteAccountCommand = new DelegateCommand(async () => await DeleteAccount());
            SaveEditedAccountCommand = new DelegateCommand(async () => await SaveEditedAccount(), CanSaveEditedAccount).ObservesProperty(() => IsSaveEditedAccountBtnEnabled);
            AddNewAccountCommand = new DelegateCommand(async () => await AddNewAccount(), CanAddNewAccount).ObservesProperty(() => IsAddNewAccountBtnEnabled);
        }

        private bool CanSaveEditedAccount()
        {
            List<bool> checkResults = new()
            {
                PropertyHasErrors(nameof(EditAccountName)),
                PropertyHasErrors(nameof(EditAccountTraderFeeString)),
            };

            return checkResults.TrueForAll(cr => cr);
        }

        private bool CanAddNewAccount()
        {
            List<bool> checkResults = new()
            {
                PropertyHasErrors(nameof(NewAccountName)),
                PropertyHasErrors(nameof(NewAccountTraderFeeString))
            };

            return checkResults.TrueForAll(cr => cr);
        }

        private async Task SaveEditedAccount()
        {

        }

        private async Task DeleteAccount()
        {
            
        }

        private async Task EditAccount()
        {

        }

        private async Task SwitchAccount()
        {
            
        } 

        private async Task AddNewAccount()
        {

        }
    }
}
