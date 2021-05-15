using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Interfaces
{
    public interface IExternalDataManager
    {
        public Task<IEnumerable<OpenTrade>> ImportTradesHistory(string path);

        public Task ExportClosedTrades(IEnumerable<ClosedTrade> trades, string filename);
    }
}
