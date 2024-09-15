using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Warehouse.UnitTests.Validations.Category
{
    public class CreateCategoryValidatorTest : IClassFixture<CreateCategoryValidatorTestFixture>
    {
        private readonly CreateCategoryValidatorTestFixture _baseFixture;

        public CreateCategoryValidatorTest(CreateCategoryValidatorTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void WhenValidDataThenSuccess()
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateCategoryRequest(fixture);
            var validator = _baseFixture.BuildCreateCategoryValidator(fixture);

            // act
            var actualResult = validator.TestValidate(model);
            validator.ValidateAndThrow(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WhenNameIsNullOrEmptyThenFail(string value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateCategoryRequest(fixture);
            var validator = _baseFixture.BuildCreateCategoryValidator(fixture);

            model.Name = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();

            actualResult.ShouldHaveValidationErrorFor(x => x.Name).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(5)]
        public void WhenLowStockIsLessThanOutOfStockThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateCategoryRequest(fixture);
            var validator = _baseFixture.BuildCreateCategoryValidator(fixture);

            model.OutOfStock = value;
            model.LowStock = --value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.LowStock).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(5)]
        public void WhenLowStockIsEqualToOutOfStockThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateCategoryRequest(fixture);
            var validator = _baseFixture.BuildCreateCategoryValidator(fixture);

            model.OutOfStock = value;
            model.LowStock = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.LowStock).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(-1)]
        public void WhenOutOfStockIsLessThanZeroThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateCategoryRequest(fixture);
            var validator = _baseFixture.BuildCreateCategoryValidator(fixture);

            model.OutOfStock = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.OutOfStock).WithSeverity(Severity.Error);
        }
    }
}
