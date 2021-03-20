using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeStats.Models.Domain;

namespace TradeStats.Services.Persistance.Configurations
{
    class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Exchange)
                .HasConversion<int>();
        }
    }
}
