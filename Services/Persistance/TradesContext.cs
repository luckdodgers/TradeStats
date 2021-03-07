using Microsoft.EntityFrameworkCore;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Infastructure.Persistance
{
    class TradesContext : DbContext, ITradesContext
    {
        public DbSet<Trade> Trades { get; set; }
        public DbSet<ClosedTrade> ClosedTrades { get; set; }

        public TradesContext(DbContextOptions<TradesContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Database.EnsureCreated())
                Database.Migrate();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
