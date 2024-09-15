using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public record CreateCategoryCommand(CreateCategoryRequest category) : IRequest<int>;
}
