using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Views.Main;

namespace TradeStats.ViewModel.MainWindow.Tabs
{
    class TradesMergeTabViewModel : BindableBase
    {
        public TradesMergeTabViewModel()
        {
            MergeCommand = new DelegateCommand(async () => await Merge(), CanMerge).ObservesProperty(() => IsMergeBtnEnabled);
            UncheckAllCommand = new DelegateCommand(async () => await UncheckAll());
        }

        public ObservableCollection<TradeMergeItemDto> TradeMergeItems { get; set; } = new ObservableCollection<TradeMergeItemDto>();

        #region Commands
        public ICommand MergeCommand { get; private set; }
        public ICommand UncheckAllCommand { get; private set; }
        #endregion

        #region IsMergeBtnEnabled
        private bool _isMergeBtnEnabled;
        public bool IsMergeBtnEnabled
        {
            get => _isMergeBtnEnabled;
            set => SetProperty(ref _isMergeBtnEnabled, value);
        }
        #endregion

        private async Task Merge()
        {
            TradeMergeItems.Add(new TradeMergeItemDto()
            {
                Id = 1,
                Side = "Buy",
                Sum = "100",
                Date = "21-12-2021",
                IsChecked = false,
                Pair = "BTC/USD",
                Price = "142000",
                Result = "0.123456"
            });
        }

        private bool CanMerge()
        {
            return true;
        }

        private async Task UncheckAll()
        {

        }
    }
}
