using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.Events;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems = [];
        public Guid CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Price TotalPrice { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems { get; private set; }

        protected Order()
        {
        }

        public Order(Guid id, Guid customerId, string customerName, Address shippingAddress) : this()
        {
            Id = id;
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Submitted;
            TotalPrice = new Price();
            OrderItems = _orderItems;

            AddDomainEvent(new OrderPlacedDomainEvent(id, customerId, customerName, TotalPrice.Amount, TotalPrice.Currency, shippingAddress));
        }

        public void CalculateOrderTotalPrice()
        {
            if (_orderItems == null || !_orderItems.Any())
            {
                TotalPrice = new Price();
                throw new Exception("Order has no items to calculate total price.");
            }

            var currency = _orderItems.First().Price.Currency;
            decimal totalAmount = 0;

            foreach (var item in _orderItems)
            {
                if (item.Price.Currency != currency)
                {
                    throw new Exception("All order items must have the same currency.");
                }
                totalAmount += item.Price.Amount * item.Quantity;
            }

            TotalPrice = new Price(currency, totalAmount);
        }

        public void AddOrderItem(Guid productId, Price price, int quantity)
        {
            // make sure there's only 1 currency in an Order.
            // might figure out a way to conver currency later.
            var firstItemCurrency = _orderItems.FirstOrDefault()?.Price.Currency;

            if (firstItemCurrency is not null && price.Currency != firstItemCurrency)
            {
                throw new Exception($"Order item must have {firstItemCurrency} as currency");
            }

            var existingProduct = _orderItems.FirstOrDefault(o => o.Id == productId);

            if (existingProduct is not null)
            {
                existingProduct.IncreaseQuantity(quantity);
                return;
            }

            _orderItems.Add(new OrderItem(productId, price, quantity));
            CalculateOrderTotalPrice();
        }

        public void SetPaidStatus()
        {
            if(Status != OrderStatus.Paid)
            {
                Status = OrderStatus.Paid;

                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));
            }
        }
    }
}
