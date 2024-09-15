using FluentValidation;
using WarehouseApi.Models.Category;

namespace WarehouseApi.Validators
{
    public class UpdateCategoryStockValidator : AbstractValidator<UpdateCategoryStockRequest>
    {
        public UpdateCategoryStockValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);

            RuleFor(m => m.OutOfStock).GreaterThanOrEqualTo(0);

            RuleFor(m => m.LowStock).GreaterThan(m => m.OutOfStock);
        }
    }
}
