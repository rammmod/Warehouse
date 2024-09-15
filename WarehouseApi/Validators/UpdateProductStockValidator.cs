using FluentValidation;
using WarehouseApi.Models.Product;

namespace WarehouseApi.Validators
{
    public class UpdateProductStockValidator : AbstractValidator<UpdateProductStockRequest>
    {
        public UpdateProductStockValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);

            RuleFor(m => m.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
