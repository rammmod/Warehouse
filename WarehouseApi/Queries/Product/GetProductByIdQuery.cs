using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Product
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDTO>;
}
