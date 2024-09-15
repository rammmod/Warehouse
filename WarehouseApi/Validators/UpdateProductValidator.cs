using FluentValidation;
using WarehouseApi.Models.Product;

namespace WarehouseApi.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);

            RuleFor(m => m.Name).NotEmpty();

            RuleFor(m => m.CategoryId).GreaterThan(0);
        }
    }
}
