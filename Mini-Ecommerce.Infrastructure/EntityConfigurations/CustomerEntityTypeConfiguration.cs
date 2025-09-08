using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("customers");

            builder.Ignore(c => c.DomainEvents);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .HasMaxLength(200);

            builder.OwnsOne(c => c.Address);
        }
    }
}
