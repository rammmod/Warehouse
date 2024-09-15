using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public record DeleteCategoryCommand(DeleteCategoryRequest category) : IRequest<bool>;
}
