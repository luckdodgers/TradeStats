using System;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.ViewModel.ManageAccounts
{
    class ManageAccountsViewModel : BaseViewModel, IDisposable
    {
        private readonly ITradesContext _tradesContext;
        private readonly IUpdateCachedData<Account> _updateCachedData;
        private readonly ICachedData<Account> _cachedData;

        private readonly EditAccountViewModel _editAccountViewModel;
        public EditAccountViewModel EditAccountVM => _editAccountViewModel;

        private readonly NewAccountViewModel _newAccountViewModel;
        public NewAccountViewModel NewAccountVM => _newAccountViewModel;

        public ManageAccountsViewModel(ITradesContext tradesContext, IUpdateCachedData<Account> updateCachedData, ICachedData<Account> cachedData)
        {
            _tradesContext = tradesContext;
            _updateCachedData = updateCachedData;
            _cachedData = cachedData;

            _editAccountViewModel = new EditAccountViewModel(_tradesContext, _updateCachedData, _cachedData);
            _newAccountViewModel = new NewAccountViewModel(_tradesContext);

            NewAccountVM.NewAccountAdd += EditAccountVM.OnNewAccountAdd;
        }

        public void Dispose()
        {
            NewAccountVM.NewAccountAdd -= EditAccountVM.OnNewAccountAdd;
        }
    }
}
