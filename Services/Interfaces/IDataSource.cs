using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeStats.Services.Interfaces
{
    public interface IDataSource<T> where T : class
    {
        Task<IEnumerable<T>> LoadData();
    }
}
