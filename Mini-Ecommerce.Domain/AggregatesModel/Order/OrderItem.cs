using Mini_Ecommerce.Domain.AggregatesModel.ProductAggregate;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;

namespace Mini_Ecommerce.Domain.AggregatesModel.Order
{
    public class OrderItem : Entity
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public Sku Sku { get; private set; }
        public Price Price{ get; private set; }
        public int Quantity {  get; private set; }
        public Price LineTotal { get; private set; }

        protected OrderItem() { }

        public OrderItem(Guid id, Guid productId, string productName, Price price, int quantity)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            LineTotal = new Price(price.Currency, price.Amount * quantity);
        }

        public OrderItem(Guid productId, Price price, int quantity)
        {
            ProductId = productId;
            Price = price;
            Quantity = quantity;
            LineTotal = new Price(price.Currency, price.Amount * quantity);
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public static OrderItem Add(Guid productId, string productName, Price price, int quantity)
        {
            if(quantity <= 0)
            {
                throw new Exception("Invalid quanitty, must be greater than 0 ");
            }

            var orderItem = new OrderItem(Guid.NewGuid(), productId, productName, price, quantity);
            return orderItem;
        }
    }
}
