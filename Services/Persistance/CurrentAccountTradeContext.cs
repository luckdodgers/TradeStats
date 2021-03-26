using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.Persistance
{
    class CurrentAccountTradeContext : ICurrentAccountTradeContext
    {
        private readonly ITradesContext _context;
        private readonly ICachedData<Account> _curAccountCache;

        public CurrentAccountTradeContext(ITradesContext context, ICachedData<Account> curAccountCache)
        {
            _context = context;
            _curAccountCache = curAccountCache;
        }

        public ITradesContext TradesContext => _context;

        public IQueryable<OpenTrade> CurrentAccountOpenTrades => _context.OpenTrades.Where(t => t.AccountId == _curAccountCache.CurrentAccount.Id);
        public IQueryable<ClosedTrade> CurrentAccountClosedTrades => _context.ClosedTrades.Where(t => t.AccountId == _curAccountCache.CurrentAccount.Id);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}
