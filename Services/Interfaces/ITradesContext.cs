using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    interface ITradesContext
    {
        DbSet<OpenTrade> Trades { get; }
        DbSet<ClosedTrade> ClosedTrades { get; }
        DbSet<Account> Accounts { get; }

        //void SetCurrentAccountId(int? id);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
