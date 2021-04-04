using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.ViewModel.DTO;

namespace TradeStats.Models.Rules
{
    public static class ClosedTradesDataGridSortingRule
    {
        public static void SetWithDataGridSorting(this ObservableCollection<ClosedTradeItemDto> collection, IEnumerable<ClosedTradeItemDto> newValue)
        {
            var orderedNewValue = newValue.OrderBy(t => t.Pair)
                .ThenBy(t => t.Date)
                .ThenBy(t => t.ProfitPerTrade);

            collection.Clear();
            collection.AddRange(orderedNewValue);
        }
    }
}
