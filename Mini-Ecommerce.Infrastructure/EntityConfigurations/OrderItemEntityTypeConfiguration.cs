using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems", "ecommerce");

            // Primary key
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                .ValueGeneratedNever();

            // Foreign key to Order (shadow property)
            builder.Property<Guid>("OrderId")
                .IsRequired();

            // Properties
            builder.Property(oi => oi.ProductId)
                .IsRequired();

            builder.Property(oi => oi.ProductName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            // Configure Price as owned entity
            builder.OwnsOne(oi => oi.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("UnitPrice")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            // Configure LineTotal as owned entity
            builder.OwnsOne(oi => oi.LineTotal, lineTotalBuilder =>
            {
                lineTotalBuilder.Property(p => p.Currency)
                    .HasColumnName("LineTotalCurrency")
                    .HasMaxLength(3)
                    .IsRequired();

                lineTotalBuilder.Property(p => p.Amount)
                    .HasColumnName("LineTotalAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            // Index on ProductId
            builder.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("IX_OrderItems_ProductId");
        }
    }
}
