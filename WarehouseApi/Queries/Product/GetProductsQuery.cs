using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Product
{
    public record GetProductsQuery : IRequest<IList<ProductDTO>>;
}
