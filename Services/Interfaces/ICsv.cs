using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeStats.Services.Interfaces
{
    public interface ICsv
    {
        public Task<IAsyncEnumerable<T>> ReadAsync<T>(string path) where T : class;

        public Task WriteAsync<T>(IEnumerable<T> data, string filename, string path) where T : class;
    }
}