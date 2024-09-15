using WarehouseApi.Models.Product;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public record UpdateProductStockCommand(UpdateProductStockRequest product) : IRequest<bool>;
}
