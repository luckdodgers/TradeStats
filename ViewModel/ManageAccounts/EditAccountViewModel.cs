using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeStats.Models.Domain;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Validations;

namespace TradeStats.ViewModel.ManageAccounts
{
    class EditAccountViewModel : BaseViewModel
    {
        public ObservableCollection<string> ExistingAccounts { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> EditAccountExchange { get; set; } = new ObservableCollection<string>();

        #region Commands
        public ICommand SwitchAccountCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }
        public ICommand SaveEditedAccountCommand { get; private set; }
        #endregion

        #region IsSwitchAccountBtnEnabled
        private bool _isSwitchAccountBtnEnabled;
        public bool IsSwitchAccountBtnEnabled
        {
            get => _isSwitchAccountBtnEnabled;
            set => SetProperty(ref _isSwitchAccountBtnEnabled, value);
        }
        #endregion

        #region IsEditAccountBtnEnabled
        private bool _isEditAccountBtnEnabled;
        public bool IsEditAccountBtnEnabled
        {
            get => _isEditAccountBtnEnabled;
            set => SetProperty(ref _isEditAccountBtnEnabled, value);
        }
        #endregion

        #region IsDeleteAccountBtnEnabled
        private bool _isDeleteAccountBtnEnabled;
        public bool IsDeleteAccountBtnEnabled
        {
            get => _isDeleteAccountBtnEnabled;
            set => SetProperty(ref _isDeleteAccountBtnEnabled, value);
        }
        #endregion

        #region IsSaveEditedAccountBtnEnabled
        private bool _isSaveEditedAccountBtnEnabled;
        public bool IsSaveEditedAccountBtnEnabled
        {
            get => _isSaveEditedAccountBtnEnabled;
            set => SetProperty(ref _isSaveEditedAccountBtnEnabled, value);
        }
        #endregion

        #region SelectedAccount
        private string _selectedAccount;
        public string SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                ValidateProperty(value);
                SetProperty(ref _selectedAccount, value);
                CheckIfCurrentAccountSelected();
            }
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
                SetProperty(ref _editAccountName, value);
                CheckIfCanSaveEditedAccount();
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
                SetProperty(ref _editAccountTraderFeeString, value);
                CheckIfCanSaveEditedAccount();
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

        #region Validations
        private readonly PositivePercentValidation _traderFeeValidation = new();
        private readonly NotEmptyStringValidation _accountNameValidation = new();
        #endregion

        private readonly ITradesContext _context;
        private readonly IUpdateCachedData<Account> _currentCachedAccount;

        public EditAccountViewModel(ITradesContext context, IUpdateCachedData<Account> currentCachedAccount)
        {
            _context = context;
            _currentCachedAccount = currentCachedAccount;

            InitValidators();
            InitCommands();
            InitBindings();
        }

        private void InitValidators()
        {
            AddValidators(nameof(SelectedAccount), _accountNameValidation);
            AddValidators(nameof(EditAccountName), _accountNameValidation);
            AddValidators(nameof(EditAccountTraderFeeString), _traderFeeValidation);
        }

        private void InitCommands()
        {
            SwitchAccountCommand = new DelegateCommand(async () => await SwitchAccount());
            EditAccountCommand = new DelegateCommand(async () => await EditAccount());
            DeleteAccountCommand = new DelegateCommand(async () => await DeleteAccount());
            SaveEditedAccountCommand = new DelegateCommand(async () => await SaveEditedAccount()).ObservesCanExecute(() => IsSaveEditedAccountBtnEnabled);
        }

        public void OnNewAccountAdd(string newAccount) => ExistingAccounts.Add(newAccount);

        // Raise on constructor to force fields validation
        private void InitBindings()
        {
            EditAccountName = string.Empty;
            EditAccountTraderFeeString = string.Empty;

            var accounts = _context.Accounts.AsNoTracking().Select(a => a.AccountName);
            ExistingAccounts.AddRange(accounts);
        }

        private void CheckIfCanSaveEditedAccount()
            => IsSaveEditedAccountBtnEnabled = NoPropertiesValidationErrors(nameof(EditAccountName), nameof(EditAccountTraderFeeString));

        private void CheckIfCurrentAccountSelected()
        {
            var isSelected = NoPropertiesValidationErrors(nameof(SelectedAccount));

            IsSwitchAccountBtnEnabled = isSelected;
            IsEditAccountBtnEnabled = isSelected;
            IsDeleteAccountBtnEnabled = isSelected;
        }

        private async Task SaveEditedAccount()
        {
            var account = await _context.Accounts.FirstAsync(a => a.AccountName == SelectedAccount);

            account.RenameAccount(EditAccountName);
            account.SwitchExchange(SelectedEditAccountExchange);
            account.ChangeFee(decimal.Parse(EditAccountTraderFeeString));

            if (!IsNewFeeStartsFromNextTrade)
            {
                var test = await _context.OpenTrades
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

                if (ExistingAccounts.Count == 0)
                    SelectedAccount = string.Empty;

                if (((ICachedData<Account>)_currentCachedAccount).CurrentAccount?.Id == accountToDelete.Id)
                    _currentCachedAccount.UpdateCache(null);

                var tradesToDelete = await _context.OpenTrades.Where(t => t.AccountId == accountToDelete.Id).ToListAsync();
                var closedTradesToDelete = await _context.ClosedTrades.Where(ct => ct.AccountId == accountToDelete.Id).ToListAsync();

                _context.OpenTrades.RemoveRange(tradesToDelete);
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
            _currentCachedAccount.UpdateCache(switchedAccount);

            await _context.SaveChangesAsync();
        }
    }
}
