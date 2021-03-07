using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Validations;
using TradeStats.Models.Settings;

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

        #region IsAddNewAccountBtnEnabled
        private bool _isAddNewAccountBtnEnabled;
        public bool IsAddNewAccountBtnEnabled
        {
            get => _isAddNewAccountBtnEnabled;
            set => SetProperty(ref _isAddNewAccountBtnEnabled, value);
        }
        #endregion

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
            set
            {
                ValidateProperty(value);
                CanAddNewAccount();
                SetProperty(ref _editAccountName, value);
            }
        }
        #endregion

        #region SelectedEditAccountExchange
        private Exchanges _selectedEditAccountExchange;
        public Exchanges SelectedEditAccountExchange
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
                CanAddNewAccount();
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
        private string _newAccountName = string.Empty;
        public string NewAccountName
        {
            get => _newAccountName;
            set
            {
                ValidateProperty(value);
                SetProperty(ref _newAccountName, value);
                CanAddNewAccount();
            }
        }
        #endregion

        #region SelectedNewAccountExchange
        private Exchanges _selectedNewAccountExchange;
        public Exchanges SelectedNewAccountExchange
        {
            get => _selectedNewAccountExchange;
            set => SetProperty(ref _selectedNewAccountExchange, value);
        }
        #endregion

        #region NewAccountTraderFee
        private string _newAccountTraderFeeString = string.Empty;

        public string NewAccountTraderFeeString
        {
            get => _newAccountTraderFeeString;
            set
            {
                ValidateProperty(value);
                CanAddNewAccount();
                SetProperty(ref _newAccountTraderFeeString, value);
            }
        }
        #endregion

        #region Validations
        private readonly PositivePercentValidation _traderFeeValidation = new();
        private readonly NotEmptyStringValidation _accountNameValidation = new();
        #endregion

        private readonly ISettingsProvider _settings;

        public ManageAccountsViewModel(ISettingsProvider settings)
        {
            _settings = settings;

            InitValidators();
            InitCommands();
        }

        private void InitValidators()
        {
            AddValidators(nameof(EditAccountName), _accountNameValidation);
            AddValidators(nameof(EditAccountTraderFeeString), _traderFeeValidation);

            AddValidators(nameof(NewAccountName), _accountNameValidation);
            AddValidators(nameof(NewAccountTraderFeeString), _traderFeeValidation);
        }

        private void InitCommands()
        {
            SwitchAccountCommand = new DelegateCommand(async () => await SwitchAccount());
            EditAccountCommand = new DelegateCommand(async () => await EditAccount());
            DeleteAccountCommand = new DelegateCommand(async () => await DeleteAccount());
            SaveEditedAccountCommand = new DelegateCommand(async () => await SaveEditedAccount()).ObservesCanExecute(() => IsSaveEditedAccountBtnEnabled);
            AddNewAccountCommand = new DelegateCommand(async () => await AddNewAccount()).ObservesCanExecute(() => IsAddNewAccountBtnEnabled);
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

            IsAddNewAccountBtnEnabled = checkResults.TrueForAll(cr => cr == false);

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
            var accountsData = await _settings.LoadAccountsDataAsync();
            if (accountsData.IsAccountAlreadyExist(NewAccountName))
            {
                System.Windows.MessageBox.Show("Account with this name already exists. Please, specify another name.", "Error");
                return;
            }           

            var feeData = new FeeData(DateTime.MinValue, decimal.Parse(NewAccountTraderFeeString));

            accountsData.Accounts.Add(new AccountJson
                (
                    NewAccountName,
                    SelectedNewAccountExchange,
                    new List<FeeData>() { feeData })
                );

            await _settings.WriteAccountsDataAsync(accountsData);
        }
    }
}
