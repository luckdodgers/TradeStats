using TradeStats.ViewModel.DTO;

namespace TradeStats.ViewModel.Interfaces
{
    public interface ITradesMergeTabValidations
    {
        bool IsAddToMergePossibe(TradeMergeItemDto tradeDto);
    }
}
