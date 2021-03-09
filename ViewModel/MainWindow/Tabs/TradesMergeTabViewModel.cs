using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;
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

        private readonly ICachedData<Account> _accountCache;
        private readonly ITradesContext _context;

        public TradesMergeTabViewModel(ICachedData<Account> accountCache)
        {
            _accountCache = accountCache;
        }

        private async Task LoadData()
        {
            var openedTrades = _context.Trades.Where(t => t.AccountId == _accountCache.CurrentAccount.Id && !t.IsClosed).ToList();
        }

        private async Task Merge()
        {
            
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
