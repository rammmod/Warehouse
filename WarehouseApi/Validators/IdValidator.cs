using FluentValidation;

namespace WarehouseApi.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(m => m).GreaterThan(0);
        }
    }
}
