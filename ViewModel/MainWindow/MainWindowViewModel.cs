using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Models.Data;
using TradeStats.ViewModel.MainWindow.Tabs;

namespace TradeStats.ViewModel.MainWindow
{
    class MainWindowViewModel : BindableBase
    {
        private TradesMergeTabViewModel _tradesMergeTab = new TradesMergeTabViewModel();
        public TradesMergeTabViewModel TradesMergeTab => _tradesMergeTab;

    }
}
