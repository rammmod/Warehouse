using AutoFixture;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Models.Order;
using WarehouseApi.Validators;

namespace Warehouse.UnitTests.Validations.Order
{
    public class CreateOrderValidatorTestFixture : BaseFixture
    {
        #region Builder methods

        public CreateOrderValidator BuildCreateOrderValidator(IFixture fixture)
        {
            var validator = fixture.Build<CreateOrderValidator>()
                .OmitAutoProperties()
                .Create();

            return validator;
        }

        public CreateOrderRequest BuildCreateOrderRequest(IFixture fixture)
        {
            var command = fixture.Build<CreateOrderRequest>()
                .Create();

            return command;
        }

        #endregion
    }
}
