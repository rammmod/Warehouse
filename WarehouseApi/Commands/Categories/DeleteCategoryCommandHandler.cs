using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Exceptions;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        readonly IValidator<int> _validator;
        readonly ICategoryRepository _categoryRepository;
        readonly IProductRepository _productRepository;

        public DeleteCategoryCommandHandler(
            IValidator<int> validator,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.category.Id, cancellationToken);

            _ = await _categoryRepository.GetAsync(request.category.Id, cancellationToken) ?? throw new CategoryNotFoundException();

            if (await _productRepository.ExistsInCategory(request.category.Id, cancellationToken))
                throw new CategoryNotEmptyException();

            return await _categoryRepository.DeleteAsync(request.category.Id, cancellationToken);
        }
    }
}
