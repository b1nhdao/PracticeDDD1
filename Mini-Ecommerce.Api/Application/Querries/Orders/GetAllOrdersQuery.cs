using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Orders
{
    public class GetAllOrdersQuery : IRequest<PagedResponse<Order>>
    {
    }
}
