using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReportsConverter.Extensions;
using TradeReportsConverter.Models;
using TradeStats.Models.Domain;
using TradeStats.Models.Import;
using TradeStats.Services.Interfaces;
using TradeStats.Extensions;

namespace TradeStats.Services.DataImport
{
    class CsvDataImport : ICsvImport<OpenTrade>, IDisposable
    {
        private FileStream _fileStream;
        private StreamReader _streamReader;
        private CsvReader _csvReader;

        private readonly ICachedData<Account> _curAccount;

        public CsvDataImport(ICachedData<Account> curAccount)
        {
            _curAccount = curAccount;
        }

        public async Task<IEnumerable<OpenTrade>> LoadData(string path)
        {
            var rawCsvData = await ParseAsync(path);
            return await ConvertToTrades(rawCsvData);
        }

        private async Task<IAsyncEnumerable<RawCsvData>> ParseAsync(string path)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            if (!File.Exists(path))
                throw new FileNotFoundException("File \"trades.csv\" not found in current directory.");

            _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _streamReader = new StreamReader(_fileStream);
            _csvReader = new CsvReader(_streamReader, config);

            return await Task.Run(() => _csvReader.GetRecordsAsync<RawCsvData>());
        }

        private async Task<List<OpenTrade>> ConvertToTrades(IAsyncEnumerable<RawCsvData> readData)
        {
            var result = new List<OpenTrade>(50);

            await foreach (var entry in readData)
            {
                var trade = new OpenTrade
                    (
                        accountId: _curAccount.CurrentAccount.Id,
                        datetime: entry.Time,
                        type: entry.Type.ParseToSideEnum(),
                        firstCurrency: entry.Pair.GetCurrencyEnumPair().Item1,
                        secondCurrency: entry.Pair.GetCurrencyEnumPair().Item2,
                        price: entry.Price,
                        amount: entry.Vol,
                        sum: entry.Cost,
                        feeCurency: Currency.USD,
                        fee: entry.Fee
                    );

                result.Add(trade);
            }

            return result;
        }

        private async Task WriteToCsv(IEnumerable<TradeHistoryData> readyDataCollection)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Report");

            Directory.CreateDirectory(path);

            using (var writer = new StreamWriter(File.Open(Path.Combine(path, "report.csv"), FileMode.OpenOrCreate, FileAccess.Write)))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(readyDataCollection);
            }
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            _streamReader?.Dispose();
            _csvReader?.Dispose();
        }
    }
}
