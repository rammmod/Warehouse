using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public record UpdateCategoryCommand(UpdateCategoryRequest category) : IRequest<bool>;
}
