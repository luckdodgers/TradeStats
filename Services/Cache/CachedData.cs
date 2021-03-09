using System;
using System.Linq;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.Cache
{
    class CachedData : IUpdateCachedData<Account>, ICachedData<Account>
    {
        public Account CurrentAccount { get; private set; } = null;

        private readonly ITradesContext _context;

        public event Action CacheUpdated;

        public CachedData(ITradesContext context)
        {
            _context = context;

            CurrentAccount = _context.Accounts.FirstOrDefault(a => a.IsActive);
        }

        public void UpdateCurrentAccount(Account account)
        {
            CurrentAccount = account;
            CacheUpdated?.Invoke();
        }
    }
}
