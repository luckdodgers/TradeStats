using System.Threading.Tasks;
using TradeStats.Models.Settings;

namespace TradeStats.Services.Interfaces
{
    interface ISettingsProvider
    {
        Task WriteAccountsDataAsync(AccountsData data);
        Task<AccountsData> LoadAccountsDataAsync();
        AccountsData LoadAccountsData();
    }
}
