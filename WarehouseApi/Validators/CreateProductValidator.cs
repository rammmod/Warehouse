using FluentValidation;
using WarehouseApi.Models.Product;

namespace WarehouseApi.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(m => m.Name).NotEmpty();

            RuleFor(m => m.Stock).GreaterThanOrEqualTo(0);

            RuleFor(m => m.CategoryId).GreaterThan(0);
        }
    }
}
