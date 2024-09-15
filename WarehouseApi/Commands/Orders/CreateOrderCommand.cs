using WarehouseApi.Models.Order;
using MediatR;

namespace WarehouseApi.Commands.Orders
{
    public record CreateOrderCommand(CreateOrderRequest order) : IRequest<Unit>;
}
