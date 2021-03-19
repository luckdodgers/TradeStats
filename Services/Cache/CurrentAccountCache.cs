using System;
using System.Linq;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.Cache
{
    class CurrentAccountCache : ICachedData<Account>, IUpdateCachedData<Account>
    {
        public Account CurrentAccount { get; private set; } = null;

        private readonly ITradesContext _context;

        public event Action CacheUpdated;

        public CurrentAccountCache(ITradesContext context)
        {
            _context = context;

            CurrentAccount = _context.Accounts.FirstOrDefault(a => a.IsActive);
        }

        public void UpdateCache(Account account)
        {
            CurrentAccount = account;
            CacheUpdated?.Invoke();
        }
    }
}
