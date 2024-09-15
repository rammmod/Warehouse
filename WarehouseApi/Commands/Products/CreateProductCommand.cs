using WarehouseApi.Models.Product;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public record CreateProductCommand(CreateProductRequest product) : IRequest<int>;
}
