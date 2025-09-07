using Mini_Ecommerce.Domain.AggregatesModel.ValueObjects;
using Mini_Ecommerce.Domain.SeedWork;

namespace Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Address Address { get; private set; }

        public Customer(Guid id, string name, string email, Address address) : this()
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
        }

        protected Customer() { }

        public static Customer Add(string name, string email, Address address)
        {
            var customer = new Customer(Guid.NewGuid(), name, email, address);
            return customer;
        }

    }
}
