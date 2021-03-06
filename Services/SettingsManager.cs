using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TradeStats.Models.Settings;

namespace TradeStats.Services
{
    class SettingsManager
    {
        private readonly string _accountSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), @"Settings\accounts.json");

        public async Task WriteAccountsData(AccountsData data)
        {
            using (FileStream fs = new FileStream(_accountSettingsPath, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fs, data);  
        }

        public async Task<AccountsData> LoadAccountsData()
        {
            using (FileStream fs = new FileStream(_accountSettingsPath, FileMode.OpenOrCreate))
                return await JsonSerializer.DeserializeAsync<AccountsData>(fs);
        }
    }
}
