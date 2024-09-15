using FluentValidation;
using WarehouseApi.Models.Category;

namespace WarehouseApi.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidator()
        {
            RuleFor(m => m.Name).NotEmpty();

            RuleFor(m => m.OutOfStock).GreaterThanOrEqualTo(0);

            RuleFor(m => m.LowStock).GreaterThan(m => m.OutOfStock);
        }
    }
}
