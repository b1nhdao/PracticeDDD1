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
        public decimal TotalPrice { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        internal Order()
        {
        }

        public Order(Guid customerId, string customerName, OrderStatus status, List<OrderItem> orderItems)
        {
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = status;
            _orderItems = new List<OrderItem>();
            foreach (var item in orderItems)
            {
                _orderItems.Add(item);
                TotalPrice += item.Price * item.Quantity;
            }

            AddDomainEvent(new OrderPlacedDomainEvent(this.Id, customerId, customerName, TotalPrice));
        }

        public static Order AddOrder(Guid customerId, string customerName, OrderStatus status, List<OrderItem> orderItems)
        {
            var order = new Order(customerId, customerName, status, orderItems);
            return order;
        }

        public void Update(OrderStatus orderStatus, List<OrderItem> orderItems)
        {
            Status = orderStatus;

            // chinh lai trong ef core 
            // lay tracking het dong order items da co ra, neu notExist thi add moi vao.
            //_orderItems.Clear();
            _orderItems.AddRange(orderItems);
            CalculateOrderTotalPrice();
        }

        public void SetOrderItems(List<OrderItem> orderItems)
        {
            _orderItems.Clear();
            foreach (var item in orderItems)
            {
                AddOrderItem(item);
            }
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            var existingOrderItem = _orderItems.Find(o => o.ProductId == orderItem.ProductId);

            if (existingOrderItem is not null)
            {
                _orderItems.Remove(existingOrderItem);
                orderItem.IncreaseQuantity(orderItem.Quantity);
            }
            _orderItems.Add(orderItem);
        }

        public void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        public void CalculateOrderTotalPrice()
        {
            TotalPrice = 0;
            foreach (var item in _orderItems)
            {
                TotalPrice += item.Price * item.Quantity;
            }
        }

        public void AddOrderItem(Guid productId, string productName, decimal price, int quantity)
        {
            var existingProduct = _orderItems.FirstOrDefault(o => o.Id == productId);

            if (existingProduct is not null)
            {
                existingProduct.IncreaseQuantity(quantity);
                return;
            }

            _orderItems.Add(new OrderItem(Guid.NewGuid(), this.Id, productId, productName, price, quantity));
            CalculateOrderTotalPrice();
        }

        public void SetPaidStatus()
        {
            if (Status != OrderStatus.Paid)
            {
                Status = OrderStatus.Paid;

                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));
            }
        }
    }
}
