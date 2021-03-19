using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;
using TradeStats.Models.Common.Comparers;

namespace TradeStats.Services.DataImport
{
    class OpenTradesLoader : IOpenTradesLoader
    {
        private readonly ITradesContext _context;
        private readonly ICachedData<Account> _curAccountCache;

        public OpenTradesLoader(ITradesContext context, ICachedData<Account> curAccountCache)
        {
            _context = context;
            _curAccountCache = curAccountCache;
        }

        public async Task UpdateOpenTrades(IEnumerable<OpenTrade> importedTrades)
        {
            var startDate = importedTrades.Min(t => t.Datetime);
            var endDate = importedTrades.Max(t => t.Datetime);

            var existingAccountTrades = await _context.OpenTrades
                .Where(t => t.AccountId == _curAccountCache.CurrentAccount.Id && t.Datetime >= startDate && t.Datetime <= endDate)
                .ToListAsync();

            var accountsToAdd = importedTrades.Except(existingAccountTrades, new OpenTradeValueComparer());

            _context.OpenTrades.AddRange(accountsToAdd);

            await _context.SaveChangesAsync();
        }
    }
}
