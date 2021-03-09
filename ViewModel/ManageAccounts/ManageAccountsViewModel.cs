using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeStats.Models.Domain;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Validations;
using TradeStats.Extensions;

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
                CheckIfCanSaveEditedAccount();
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
                CheckIfCanSaveEditedAccount();
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

        #region IsNewFeeStartDatepickerActive
        private bool _isNewFeeStartDatepickerActive = true;
        public bool IsNewFeeStartDatepickerActive
        {
            get => _isNewFeeStartDatepickerActive;
            set => SetProperty(ref _isNewFeeStartDatepickerActive, value);
        }
        #endregion

        #region IsNewFeeStartsFromNextTrade
        private bool _isNewFeeStartsFromNextTrade;
        public bool IsNewFeeStartsFromNextTrade
        {
            get => _isNewFeeStartsFromNextTrade;
            set
            {
                IsNewFeeStartDatepickerActive = !value;
                SetProperty(ref _isNewFeeStartsFromNextTrade, value);
            }
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
                CheckIfCanAddAccount();
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
                CheckIfCanAddAccount();
                SetProperty(ref _newAccountTraderFeeString, value);
            }
        }
        #endregion

        #region Validations
        private readonly PositivePercentValidation _traderFeeValidation = new();
        private readonly NotEmptyStringValidation _accountNameValidation = new();
        #endregion

        private readonly ITradesContext _context;
        private readonly IUpdateCachedData<Account> _currentAccount;

        public ManageAccountsViewModel(ITradesContext context, IUpdateCachedData<Account> currentAccount)
        {
            _context = context;
            _currentAccount = currentAccount;

            InitValidators();
            InitCommands();
            InitBindings();
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

        // Raise at start to force fields validation
        private void InitBindings()
        {
            EditAccountName = string.Empty;
            EditAccountTraderFeeString = string.Empty;

            NewAccountName = string.Empty;
            NewAccountTraderFeeString = string.Empty;

            var accounts = _context.Accounts.AsNoTracking().Select(a => a.AccountName);
            ExistingAccounts.AddRange(accounts);
        }

        private void CheckIfCanSaveEditedAccount()
        {
            List<bool> checkResults = new()
            {
                PropertyHasErrors(nameof(EditAccountName)),
                PropertyHasErrors(nameof(EditAccountTraderFeeString)),              
            };

            IsSaveEditedAccountBtnEnabled = checkResults.TrueForAll(cr => cr == false);
        }

        private void CheckIfCanAddAccount()
        {
            List<bool> checkResults = new()
            {
                PropertyHasErrors(nameof(NewAccountName)),
                PropertyHasErrors(nameof(NewAccountTraderFeeString))
            };

            IsAddNewAccountBtnEnabled = checkResults.TrueForAll(cr => cr == false);
        }

        private async Task SaveEditedAccount()
        {
            var account = await _context.Accounts.FirstAsync(a => a.AccountName == SelectedAccount);

            account.RenameAccount(EditAccountName);
            account.SwitchExchange(SelectedEditAccountExchange);
            account.ChangeFee(decimal.Parse(EditAccountTraderFeeString));

            if (!IsNewFeeStartsFromNextTrade)
            {
                var test = await _context.Trades
                    .Where(t => t.AccountId == account.Id && t.Datetime >= NewFeeStartDate)
                    .ToListAsync();

                test.ForEach(t => t.SetFee(account.Fee));
            }
                
            await _context.SaveChangesAsync();

            int index = ExistingAccounts.IndexOf(SelectedAccount);
            ExistingAccounts.RemoveAt(index);
            ExistingAccounts.Insert(index, EditAccountName);
        }

        private async Task DeleteAccount()
        {
            var answer = MessageBox.Show("Account and all it's records will be permanently removed from the database. Are you sure?",
                "Delete account?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (answer == MessageBoxResult.Yes)
            {
                var accountToDelete = await _context.Accounts.FirstAsync(a => a.AccountName == SelectedAccount);

                ExistingAccounts.Remove(accountToDelete.AccountName);
                EditAccountName = string.Empty;
                EditAccountTraderFeeString = string.Empty;

                if (((ICachedData<Account>)_currentAccount).CurrentAccount?.Id == accountToDelete.Id)
                    _currentAccount.UpdateCurrentAccount(null);

                var tradesToDelete = await _context.Trades.Where(t => t.AccountId == accountToDelete.Id).ToListAsync();
                var closedTradesToDelete = await _context.ClosedTrades.Where(ct => ct.AccountId == accountToDelete.Id).ToListAsync();

                _context.Trades.RemoveRange(tradesToDelete);
                _context.ClosedTrades.RemoveRange(closedTradesToDelete);
                _context.Accounts.Remove(accountToDelete);

                await _context.SaveChangesAsync();
            }
        }

        private async Task EditAccount()
        {
            var account = await _context.Accounts.AsNoTracking().FirstAsync(a => a.AccountName == SelectedAccount);

            EditAccountName = account.AccountName;
            SelectedEditAccountExchange = account.Exchange;
            EditAccountTraderFeeString = account.Fee.ToString();
        }

        private async Task SwitchAccount()
        {
            var accounts = await _context.Accounts.ToListAsync();

            accounts.ForEach(a => a.SetInactive());
            var switchedAccount = accounts.First(a => a.AccountName == SelectedAccount);
            switchedAccount.SetActive();
            _currentAccount.UpdateCurrentAccount(switchedAccount);

            await _context.SaveChangesAsync();
        } 

        private async Task AddNewAccount()
        {          
            if (await _context.Accounts.AsNoTracking().AnyAsync(a => a.AccountName == NewAccountName))
            {
                MessageBox.Show("Account with this name already exists. Please, specify another name.", "Error");
                return;
            }

            await _context.Accounts.AddAsync(new Account(NewAccountName, SelectedNewAccountExchange, decimal.Parse(NewAccountTraderFeeString)));
            await _context.SaveChangesAsync();

            ExistingAccounts.Add(NewAccountName);

            NewAccountName = string.Empty;
            NewAccountTraderFeeString = string.Empty;
        }
    }
}
