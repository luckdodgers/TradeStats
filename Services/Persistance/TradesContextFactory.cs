using Microsoft.EntityFrameworkCore;
using TradeStats.Infastructure.Persistance;
using TradeStats.Services.Interfaces;

namespace TradeStats.Services.Persistance
{
    class TradesContextFactory : IContextFactory<ITradesContext>
    {
        private string connectionString;

        public TradesContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ITradesContext CreateDbContext()
        {
            if (connectionString == null)
                return null;

            var optionsBuilder = new DbContextOptionsBuilder<TradesContext>();
            optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.UseLazyLoadingProxies();

            return new TradesContext(optionsBuilder.Options);
        }

        public void SwitchDatabase(string dbName)
        {
            connectionString = $"Data Source={dbName}.db";
        }
    }
}
