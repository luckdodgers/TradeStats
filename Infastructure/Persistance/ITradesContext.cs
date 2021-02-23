using Microsoft.EntityFrameworkCore;
using TradeStats.Domain.Data;

namespace TradeStats.Infastructure.Persistance
{
    interface ITradesContext
    {
        DbSet<Trade> Trades { get; }
        DbSet<ClosedTrade> ClosedTrades { get; }
    }
}
