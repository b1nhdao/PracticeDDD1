using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Infrastructure.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "ecommerce");

            // Primary key
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            // Properties
            builder.Property(o => o.CustomerId)
                .IsRequired();

            // Enum configuration
            builder.Property(o => o.Status)
                .HasConversion<int>() // Store as integer
                .IsRequired();

            // Configure TotalPrice as owned entity
            builder.OwnsOne(o => o.TotalPrice, priceBuilder =>
            {
                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();

                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("TotalAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            // Configure relationship with OrderItems
            builder.HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configure backing field for OrderItems collection
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Ignore domain events
            builder.Ignore(o => o.DomainEvents);

            // Index on CustomerId for queries
            builder.HasIndex(o => o.CustomerId)
                .HasDatabaseName("IX_Orders_CustomerId");
        }
    }
}
