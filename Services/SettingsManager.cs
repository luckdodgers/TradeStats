using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading.Tasks;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services
{
    class SettingsManager : ISettingsManager
    {
        private readonly string _accountSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), @"Settings\accounts.json");

        public async Task WriteAccountsDataAsync(AccountsData data)
        {
            using (FileStream fs = new FileStream(_accountSettingsPath, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fs, data);  
        }

        public async Task<AccountsData> LoadAccountsDataAsync()
        {
            using (FileStream fs = new FileStream(_accountSettingsPath, FileMode.Open))
                return await JsonSerializer.DeserializeAsync<AccountsData>(fs);
        }

        public AccountsData LoadAccounts()
{
            var jsonString = File.ReadAllText(_accountSettingsPath);

            return JsonSerializer.Deserialize<AccountsData>(jsonString);
        }
    }
}
