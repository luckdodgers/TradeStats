using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.ExternalData
{
    class Csv : ICsv, IDisposable
    {
        private FileStream _fileStream;
        private StreamReader _streamReader;
        private CsvReader _csvReader;

        public async Task<IAsyncEnumerable<T>> ReadAsync<T>(string path) where T : class
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

            return await Task.Run(() => _csvReader.GetRecordsAsync<T>());
        }

        public async Task WriteAsync<T>(IEnumerable<T> data, string filename, string path) where T : class
        {
            Directory.CreateDirectory(path);

            using var writer = new StreamWriter(File.Open(Path.Combine(path, $"{filename}.csv"), FileMode.OpenOrCreate, FileAccess.Write));
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(data);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            _streamReader?.Dispose();
            _csvReader?.Dispose();
        }
    }
}
