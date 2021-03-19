using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeStats.Services.Interfaces
{
    public interface ICsvImport<T> where T : class
    {
        Task<IEnumerable<T>> LoadData(string path);
    }
}