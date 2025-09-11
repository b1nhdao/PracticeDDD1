using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Customers
{
    public class GetCustomersQuery : IRequest<PagedResponse<Customer>>
    {
        public PagedRequest Parameters { get; set; }

        public GetCustomersQuery(PagedRequest parameter)
        {
            this.Parameters = parameter;
        }
    }
}
