using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Persistance.Configurations
{
    class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(b => b.AccountId);
        }
    }
}
