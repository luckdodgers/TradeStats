using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeStats.Models.Domain;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;
using TradeStats.Services.Validations;

namespace TradeStats.ViewModel.ManageAccounts
{
    class ManageAccountsViewModel : BaseViewModel, IDisposable
    {
        private readonly ITradesContext _tradesContext;
        private readonly IUpdateCachedData<Account> _updateCachedData;

        private readonly EditAccountViewModel _editAccountViewModel;
        public EditAccountViewModel EditAccountVM => _editAccountViewModel;

        private readonly NewAccountViewModel _newAccountViewModel;
        public NewAccountViewModel NewAccountVM => _newAccountViewModel;

        public ManageAccountsViewModel(ITradesContext tradesContext, IUpdateCachedData<Account> updateCachedData)
        {
            _tradesContext = tradesContext;
            _updateCachedData = updateCachedData;

            _editAccountViewModel = new EditAccountViewModel(_tradesContext, _updateCachedData);
            _newAccountViewModel = new NewAccountViewModel(_tradesContext, _updateCachedData);

            NewAccountVM.NewAccountAdd += EditAccountVM.OnNewAccountAdd;
        }

        public void Dispose()
        {
            NewAccountVM.NewAccountAdd -= EditAccountVM.OnNewAccountAdd;
        }
    }
}
