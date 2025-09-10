using Microsoft.EntityFrameworkCore;
using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;

namespace Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price{ get; private set; }
        public int Quantity {  get; private set; }
        public decimal LineTotal { get; private set; }

        protected OrderItem() { }

        public OrderItem(Guid id, Guid orderId, Guid productId, string productName, decimal price, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            LineTotal = price * quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
            LineTotal = Price * Quantity;
        }

        public static OrderItem Add(Guid productId, Guid orderId, string productName, decimal price, int quantity)
        {
            if(quantity <= 0)
            {
                throw new Exception("Invalid quanitty, must be greater than 0 ");
            }

            var orderItem = new OrderItem(Guid.NewGuid(), orderId, productId, productName, price, quantity);
            return orderItem;
        }
    }
}
