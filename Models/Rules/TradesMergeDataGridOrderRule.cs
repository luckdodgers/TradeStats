using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.ViewModel.DTO;

namespace TradeStats.Models.Rules
{
    public static class TradesMergeDataGridOrderRule
    {
        public static void SetWithDataGridSorting(this ObservableCollection<TradeMergeItemDto> collection, IEnumerable<TradeMergeItemDto> newValue)
        {
            var orderedNewValue = newValue.OrderBy(t => t.Pair)
                .ThenBy(t => t.Side)
                .ThenBy(t => t.Date);

            collection.Clear();
            collection.AddRange(orderedNewValue);
        }
    }
}
