using MediatR;
using Mini_Ecommerce.Api.Models.Pagination;
using Mini_Ecommerce.Domain.AggregatesModel.CustomerAggregate;

namespace Mini_Ecommerce.Api.Application.Querries.Customers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagedResponse<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<PagedResponse<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _customerRepository.GetPagedAsync(request.Parameters.PageIndex, request.Parameters.PageSize, request.Parameters.IsDescending);
            return new PagedResponse<Customer>(request.Parameters.PageIndex, request.Parameters.PageSize, totalCount, items);
        }
    }
}
