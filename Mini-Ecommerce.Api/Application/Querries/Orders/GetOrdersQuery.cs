using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Orders
{
    public class GetOrdersQuery : IRequest<PagedResponse<Order>>
    {
        public PagedRequest Parameters { get; set; }

        public GetOrdersQuery(PagedRequest parameters)
        {
            Parameters = parameters;
        }
    }
}
