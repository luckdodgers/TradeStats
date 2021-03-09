using System;

namespace TradeStats.Services.Interfaces
{
    public interface ICachedData<T> where T : class
    {
        T CurrentAccount { get; }
        event Action CacheUpdated;
    }
}
