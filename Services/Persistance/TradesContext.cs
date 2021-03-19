using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection;
using TradeStats.Extensions;
using TradeStats.Models.Domain;
using TradeStats.Services.Interfaces;

namespace TradeStats.Infastructure.Persistance
{
    class TradesContext : DbContext, ITradesContext
    {
        public DbSet<OpenTrade> Trades { get; set; }
        public DbSet<ClosedTrade> ClosedTrades { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public TradesContext()
        {
        }

        public TradesContext(DbContextOptions<TradesContext> options) : base(options)
        {          
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=Trades.db");
                optionsBuilder.UseLazyLoadingProxies();
            }
          
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(App)));
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseValueConverterForType(typeof(DateTime), new DateTimeToBinaryConverter());
            modelBuilder.UseValueConverterForType(typeof(decimal), new NumberToStringConverter<decimal>());
        }
    }
}
