using FluentValidation;
using WarehouseApi.Models.Order;

namespace WarehouseApi.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(m => m.ProductId).GreaterThan(0);

            RuleFor(m => m.Quantity).GreaterThan(0);

            RuleFor(m => m.Mode).IsInEnum();
        }
    }
}
