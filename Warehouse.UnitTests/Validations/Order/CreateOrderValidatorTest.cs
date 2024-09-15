using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Warehouse.UnitTests.Validations.Order
{
    public class CreateOrderValidatorTest : IClassFixture<CreateOrderValidatorTestFixture>
    {
        private readonly CreateOrderValidatorTestFixture _baseFixture;

        public CreateOrderValidatorTest(CreateOrderValidatorTestFixture baseFixture)
        {
            _baseFixture = baseFixture;
        }

        [Fact]
        public void WhenValidDataThenSuccess()
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateOrderRequest(fixture);
            var validator = _baseFixture.BuildCreateOrderValidator(fixture);

            // act
            var actualResult = validator.TestValidate(model);
            validator.ValidateAndThrow(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenProductIdEqualOrLessThanZeroThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateOrderRequest(fixture);
            var validator = _baseFixture.BuildCreateOrderValidator(fixture);

            model.ProductId = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();

            actualResult.ShouldHaveValidationErrorFor(x => x.ProductId).WithSeverity(Severity.Error);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenQuantityIsEqualOrLessThanZeroThenFail(int value)
        {
            // arrange
            var fixture = _baseFixture.BuildFixture();
            var model = _baseFixture.BuildCreateOrderRequest(fixture);
            var validator = _baseFixture.BuildCreateOrderValidator(fixture);

            model.Quantity = value;

            // act
            var actualResult = validator.TestValidate(model);

            // assert
            actualResult.Should().NotBeNull();
            actualResult.IsValid.Should().BeFalse();
            actualResult.Errors.Count.Should().Be(1);

            actualResult.ShouldHaveValidationErrorFor(x => x.Quantity).WithSeverity(Severity.Error);
        }
    }
}
