using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Category
{
    public record GetCategoriesQuery : IRequest<IList<CategoryDTO>>;
}
