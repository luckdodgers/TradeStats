using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Persistance.Configurations
{
    class TradeConfiguration : IEntityTypeConfiguration<OpenTrade>
    {
        public void Configure(EntityTypeBuilder<OpenTrade> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(b => b.AccountId);
        }
    }
}
