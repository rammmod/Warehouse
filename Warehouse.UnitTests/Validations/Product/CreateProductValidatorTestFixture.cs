using AutoFixture;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Models.Product;
using WarehouseApi.Validators;

namespace Warehouse.UnitTests.Validations.Product
{
    public class CreateProductValidatorTestFixture : BaseFixture
    {
        #region Builder methods

        public CreateProductValidator BuildCreateProductValidator(IFixture fixture)
        {
            var validator = fixture.Build<CreateProductValidator>()
                .OmitAutoProperties()
                .Create();

            return validator;
        }

        public CreateProductRequest BuildCreateProductRequest(IFixture fixture)
        {
            var command = fixture.Build<CreateProductRequest>()
                .With(x => x.Name, "Product")
                .With(x => x.Stock, 5)
                .With(x => x.CategoryId, 1)
                .Create();

            return command;
        }

        #endregion
    }
}
