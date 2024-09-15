using AutoFixture;
using Warehouse.UnitTests.Fixtures;
using WarehouseApi.Models.Category;
using WarehouseApi.Validators;

namespace Warehouse.UnitTests.Validations.Category
{
    public class CreateCategoryValidatorTestFixture : BaseFixture
    {
        #region Builder methods

        public CreateCategoryValidator BuildCreateCategoryValidator(IFixture fixture)
        {
            var validator = fixture.Build<CreateCategoryValidator>()
                .OmitAutoProperties()
                .Create();

            return validator;
        }

        public CreateCategoryRequest BuildCreateCategoryRequest(IFixture fixture)
        {
            int outOfStock = 5;

            var command = fixture.Build<CreateCategoryRequest>()
                .With(x => x.Name, "Category")
                .With(x => x.OutOfStock, outOfStock)
                .With(x => x.LowStock, ++outOfStock)
                .Create();

            return command;
        }

        #endregion
    }
}
