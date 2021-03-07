using Microsoft.EntityFrameworkCore;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    interface ITradesContext
    {
        DbSet<Trade> Trades { get; }
        DbSet<ClosedTrade> ClosedTrades { get; }
    }
}
