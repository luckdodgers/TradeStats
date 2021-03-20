using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeStats.Models.Domain;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Validations;

namespace TradeStats.ViewModel.ManageAccounts
{
    class NewAccountViewModel : BaseViewModel
    {
        public ObservableCollection<string> NewAccountExchange { get; set; } = new ObservableCollection<string>();
        public event Action<string> NewAccountAdd;

        #region Commands

        public ICommand AddNewAccountCommand { get; private set; }
        #endregion

        #region IsAddNewAccountBtnEnabled
        private bool _isAddNewAccountBtnEnabled;
        public bool IsAddNewAccountBtnEnabled
        {
            get => _isAddNewAccountBtnEnabled;
            set => SetProperty(ref _isAddNewAccountBtnEnabled, value);
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
                SetProperty(ref _newAccountTraderFeeString, value);
                CheckIfCanAddAccount();
            }
        }
        #endregion

        #region Validations
        private readonly PositivePercentValidation _traderFeeValidation = new();
        private readonly NotEmptyStringValidation _accountNameValidation = new();
        #endregion

        private readonly ITradesContext _context;

        public NewAccountViewModel(ITradesContext context)
        {
            _context = context;

            InitValidators();
            InitCommands();
            InitBindings();
        }

        private void InitValidators()
        {
            AddValidators(nameof(NewAccountName), _accountNameValidation);
            AddValidators(nameof(NewAccountTraderFeeString), _traderFeeValidation);
        }

        private void InitCommands()
        {
            AddNewAccountCommand = new DelegateCommand(async () => await AddNewAccount()).ObservesCanExecute(() => IsAddNewAccountBtnEnabled);
        }

        // Raise on constructor to force fields validation
        private void InitBindings()
        {
            NewAccountName = string.Empty;
            NewAccountTraderFeeString = string.Empty;
        }

        private void CheckIfCanAddAccount()
            => IsAddNewAccountBtnEnabled = NoPropertiesValidationErrors(nameof(NewAccountName), nameof(NewAccountTraderFeeString));

        private async Task AddNewAccount()
        {
            if (await _context.Accounts.AsNoTracking().AnyAsync(a => a.AccountName == NewAccountName))
            {
                MessageBox.Show("Account with this name already exists. Please, specify another name.", "Error");
                return;
            }

            await _context.Accounts.AddAsync(new Account(NewAccountName, SelectedNewAccountExchange, decimal.Parse(NewAccountTraderFeeString)));
            await _context.SaveChangesAsync();

            NewAccountAdd?.Invoke(NewAccountName);

            NewAccountName = string.Empty;
            NewAccountTraderFeeString = string.Empty;
        }
    }
}
