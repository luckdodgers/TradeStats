using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    public interface IOpenTradesLoader
    {
        Task UpdateOpenTrades(IEnumerable<OpenTrade> importedTrades);
    }
}
