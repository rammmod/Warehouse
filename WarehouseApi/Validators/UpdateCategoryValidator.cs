using FluentValidation;
using WarehouseApi.Models.Category;

namespace WarehouseApi.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);

            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
