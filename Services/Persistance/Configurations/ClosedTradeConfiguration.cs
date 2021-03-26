using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Persistance.Configurations
{
    class ClosedTradeConfiguration : IEntityTypeConfiguration<ClosedTrade>
    {
        public void Configure(EntityTypeBuilder<ClosedTrade> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(b => b.AccountId);

            builder.Property(b => b.FirstCurrency)
                .HasConversion<int>();

            builder.Property(b => b.SecondCurrency)
                .HasConversion<int>();

            builder.Property(b => b.Position)
                .HasConversion<int>();
        }
    }
}
