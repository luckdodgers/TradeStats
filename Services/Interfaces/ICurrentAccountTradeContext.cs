using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    public interface ICurrentAccountTradeContext
    {
        ITradesContext TradesContext { get; }
        IQueryable<OpenTrade> CurrentAccountOpenTrades { get; }
        IQueryable<ClosedTrade> CurrentAccountClosedTrades { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
