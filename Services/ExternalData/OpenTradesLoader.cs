using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeStats.Models.Domain;
using TradeStats.Models.Rules;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.ExternalData
{
    class OpenTradesLoader : IOpenTradesLoader
    {
        private readonly ICurrentAccountTradeContext _context;

        public OpenTradesLoader(ICurrentAccountTradeContext context)
        {
            _context = context;
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
                    .Except(importedTrades.Where(it => it.Datetime <= latestExistingDate)); // Remove imported trades that are earlier or equal than already existing trades in DB
            }
            
            _context.TradesContext.OpenTrades.AddRange(importedTrades);

            await _context.SaveChangesAsync();
        }
    }
}
