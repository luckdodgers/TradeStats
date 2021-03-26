using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;
using TradeStats.Models.Common.Comparers;
using TradeStats.Models.Rules;

namespace TradeStats.Services.DataImport
{
    class OpenTradesLoader : IOpenTradesLoader
    {
        private readonly ICurrentAccountTradeContext _context;
        private readonly ICachedData<Account> _curAccountCache;

        public OpenTradesLoader(ICurrentAccountTradeContext context, ICachedData<Account> curAccountCache)
        {
            _context = context;
            _curAccountCache = curAccountCache;
        }

        public async Task UpdateOpenTrades(IEnumerable<OpenTrade> importedTrades)
        {
            if (_context.CurrentAccountOpenTrades.Any() || _context.CurrentAccountClosedTrades.Any())
            {
                var latestExistingDate = _context.CurrentAccountOpenTrades
                .Select(t => t.Datetime)
                .Concat(_context.TradesContext.ClosedTrades.Select(t => t.Datetime))
                .Max();

                importedTrades = importedTrades.RemoveFiatExchanges()
                    .Except(importedTrades.Where(it => it.Datetime <= latestExistingDate)); // Remove imported trades that are earlier or equal than existing trades in DB
            }
            
            else _context.TradesContext.OpenTrades.AddRange(importedTrades);

            await _context.SaveChangesAsync();
        }
    }
}
