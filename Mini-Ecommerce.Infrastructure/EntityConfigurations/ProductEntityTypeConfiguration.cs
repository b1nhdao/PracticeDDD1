using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.Ignore(p => p.DomainEvents);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasColumnName("description").HasMaxLength(500);
            builder.Property(p => p.IsActive).HasColumnName("is_active").IsRequired();

            builder.OwnsOne(p => p.Sku);

            builder.Property(p => p.Quantity).HasColumnName("quantity").IsRequired();
        }
    }
}
