using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Warehouse.UnitTests.Validations.Product
{
    public class CreateProductValidatorTest : IClassFixture<CreateProductValidatorTestFixture>
    {
        private readonly CreateProductValidatorTestFixture _baseFixture;

        public CreateProductValidatorTest(CreateProductValidatorTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void WhenValidDataThenSuccess()
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateProductRequest(fixture);
            var validator = _baseFixture.BuildCreateProductValidator(fixture);

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
            var model = _baseFixture.BuildCreateProductRequest(fixture);
            var validator = _baseFixture.BuildCreateProductValidator(fixture);

            model.Name = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();

            actualResult.ShouldHaveValidationErrorFor(x => x.Name).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(-1)]
        public void WhenStockIsLessThanZeroThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateProductRequest(fixture);
            var validator = _baseFixture.BuildCreateProductValidator(fixture);

            model.Stock = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.Stock).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenProductIdIsNotGreaterThanZeroThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateProductRequest(fixture);
            var validator = _baseFixture.BuildCreateProductValidator(fixture);

            model.CategoryId = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.CategoryId).WithSeverity(Severity.Error);
        }
    }
}
