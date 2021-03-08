using Microsoft.EntityFrameworkCore;
using System.Linq;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Extensions
{
    static class DbSetExtensions
    {
        public static IQueryable<Trade> ForCurrentAccount(this DbSet<Trade> trades, ICachedData<Account> cached) 
            => trades.Where(t => t.AccountId == cached.CurrentAccount.Id);

        public static IQueryable<ClosedTrade> ForCurrentAccount(this DbSet<ClosedTrade> closedTrades, ICachedData<Account> cached)
            => closedTrades.Where(t => t.AccountId == cached.CurrentAccount.Id);
    }
}
