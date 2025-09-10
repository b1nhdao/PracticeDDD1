using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.OrderAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Orders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResponse<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResponse<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _orderRepository
                .GetPagedAsync(request.Parameters.PageIndex, request.Parameters.PageSize);

            return new PagedResponse<Order>(request.Parameters.PageIndex, request.Parameters.PageSize, totalCount, items);
        }
    }
}
