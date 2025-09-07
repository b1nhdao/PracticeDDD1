using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "ecommerce");

            // Primary key
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            // Properties
            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure Sku as owned entity
            builder.OwnsOne(p => p.Sku, skuBuilder =>
            {
                skuBuilder.Property(s => s.Value)
                    .HasColumnName("Sku")
                    .HasMaxLength(50)
                    .IsRequired();

                // Unique constraint on SKU
                skuBuilder.HasIndex(s => s.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Products_Sku");
            });

            // Configure Price as owned entity
            builder.OwnsOne(p => p.Price, priceBuilder =>
            {
                priceBuilder.Property(pr => pr.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                priceBuilder.Property(pr => pr.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            // Ignore domain events
            builder.Ignore(p => p.DomainEvents);

            // Index for active products
            builder.HasIndex(p => p.IsActive)
                .HasDatabaseName("IX_Products_IsActive");
        }
    }
}
