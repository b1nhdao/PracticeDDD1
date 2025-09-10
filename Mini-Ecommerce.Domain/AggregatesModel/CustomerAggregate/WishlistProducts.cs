namespace Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate
{
    public class WishlistProducts : Entity
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal ProductPrice { get; private set; }
        public DateTime DateAdded { get; private set; }

        public WishlistProducts(Guid id, Guid customerId, Guid productId, string productName, decimal productPrice, DateTime dateAdded)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            DateAdded = dateAdded;
        }
    }
}
