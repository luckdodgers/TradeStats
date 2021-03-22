using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Extensions
{
    static class DbSetExtensions
    {
        public static IQueryable<OpenTrade> ForCurrentAccount(this DbSet<OpenTrade> trades, ICachedData<Account> cached) 
            => trades.Where(t => t.AccountId == cached.CurrentAccount.Id);

        public static IQueryable<ClosedTrade> ForCurrentAccount(this DbSet<ClosedTrade> closedTrades, ICachedData<Account> cached)
            => closedTrades.Where(t => t.AccountId == cached.CurrentAccount.Id);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
