using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    public interface ITradesContext
    {
        DbSet<OpenTrade> OpenTrades { get; }
        DbSet<ClosedTrade> ClosedTrades { get; }
        DbSet<Account> Accounts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
