using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Category
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDTO>;
}
