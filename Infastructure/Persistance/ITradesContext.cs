using Microsoft.EntityFrameworkCore;
using TradeStats.Models.Domain;

namespace TradeStats.Infastructure.Persistance
{
    interface ITradesContext
    {
        DbSet<Trade> Trades { get; }
        DbSet<ClosedTrade> ClosedTrades { get; }
    }
}
