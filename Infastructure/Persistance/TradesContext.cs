using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TradeStats.Domain.Data;

namespace TradeStats.Infastructure.Persistance
{
    class TradesContext : DbContext, ITradesContext
    {
        public DbSet<Trade> Trades { get; set; }
        public DbSet<ClosedTrade> ClosedTrades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
