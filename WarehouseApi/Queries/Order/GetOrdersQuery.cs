using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Order
{
    public record GetOrdersQuery : IRequest<IList<OrderDTO>>;
}
