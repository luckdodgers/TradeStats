namespace TradeStats.Services.Interfaces
{
    public interface IUpdateCachedData<T>
    {
        void UpdateCache(T account);
    }
}
