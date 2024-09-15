using WarehouseApi.Models.Product;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public record UpdateProductCommand(UpdateProductRequest product) : IRequest<bool>;
}
