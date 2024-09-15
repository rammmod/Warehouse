using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public record UpdateCategoryStockCommand(UpdateCategoryStockRequest category) : IRequest<bool>;
}
