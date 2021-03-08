namespace TradeStats.Services.Interfaces
{
    public interface IUpdateCachedData<T>
    {
        void UpdateCurrentAccount(T account);
    }
}
