using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Persistance.Configurations
{
    class OpenTradeConfiguration : IEntityTypeConfiguration<OpenTrade>
    {
        public void Configure(EntityTypeBuilder<OpenTrade> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(b => b.AccountId);

            builder.Property(b => b.FirstCurrency)
                .HasConversion<int>();

            builder.Property(b => b.SecondCurrency)
                .HasConversion<int>();

            builder.Property(b => b.Side)
                .HasConversion<int>();
        }
    }
}
