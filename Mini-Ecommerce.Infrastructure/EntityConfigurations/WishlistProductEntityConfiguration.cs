using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    internal class WishlistProductEntityConfiguration : IEntityTypeConfiguration<WishlistProduct>
    {
        public void Configure(EntityTypeBuilder<WishlistProduct> builder)
        {
            builder.ToTable("wishlish_products");

            builder.Ignore(w => w.DomainEvents);

            builder.HasKey(w => w.Id);

            builder.HasOne<Customer>()
                .WithMany(c => c.WishlistProducts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(w => w.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(w => w.ProductPrice)
                .HasColumnName("product_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(w => w.DateAdded)
                .HasColumnName("date_added")
                .IsRequired();
        }
    }
}