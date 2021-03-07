using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TradeStats.Models.Settings;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.Persistance
{
    class JsonSettingsProvider : ISettingsProvider
    {
        private const string _filename = "accounts.json";
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Settings");
        private string _FilePath => Path.Combine(_folderPath, _filename);
        private AccountsData _accountsData = null;

        public async Task WriteAccountsDataAsync(AccountsData data)
        {
            Directory.CreateDirectory(_folderPath);

            using (FileStream fs = new FileStream(_FilePath, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync(fs, data);

            _accountsData = data;
        }

        public async Task<AccountsData> LoadAccountsDataAsync()
        {
            if (_accountsData != null)
                return await Task.FromResult(_accountsData);

            if (!File.Exists(_FilePath))
                return new AccountsData();

            using (FileStream fs = new FileStream(_FilePath, FileMode.Open))
                return await JsonSerializer.DeserializeAsync<AccountsData>(fs);
        }

        public AccountsData LoadAccounts()
        {
            if (_accountsData != null)
                return _accountsData;

            if (!File.Exists(_FilePath))
                return new AccountsData();

            var jsonString = File.ReadAllText(_FilePath);

            return JsonSerializer.Deserialize<AccountsData>(jsonString);
        }
    }
}
