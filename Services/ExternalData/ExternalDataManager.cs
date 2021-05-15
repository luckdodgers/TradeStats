using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.Models.Import;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.ExternalData
{
    class ExternalDataManager : IExternalDataManager
    {
        private readonly ICachedData<Account> _curAccount;
        private readonly IMapper _mapper;
        private readonly ICsv _csv;

        public ExternalDataManager(ICachedData<Account> curAccount, IMapper mapper, ICsv csv)
        {
            _curAccount = curAccount;
            _mapper = mapper;
            _csv = csv;
        }

        public async Task<IEnumerable<OpenTrade>> ImportTradesHistory(string path)
        {
            var rawCsvData = await _csv.ReadAsync<RawCsvData>(path);
            return await ConvertToOpenTrades(rawCsvData);
        }

        public async Task ExportClosedTrades(IEnumerable<ClosedTrade> trades, string filename)
        {
            var exportData = _mapper.Map<IEnumerable<ClosedTrade>, IEnumerable<ExportClosedTradesData>>(trades);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

            await _csv.WriteAsync(exportData, filename, path);
        }

        private async Task<List<OpenTrade>> ConvertToOpenTrades(IAsyncEnumerable<RawCsvData> readData)
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
    }
}
