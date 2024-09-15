using AutoMapper;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Order
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IList<OrderDTO>>
    {
        readonly IOrderRepository _orderRepository;
        readonly IMapper _mapper;

        public GetOrdersQueryHandler(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IList<OrderDTO>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IList<OrderDTO>>(await _orderRepository.GetAllAsync(cancellationToken));
        }
    }
}
