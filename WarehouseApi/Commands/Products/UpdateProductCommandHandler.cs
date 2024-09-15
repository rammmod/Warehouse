using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Product;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        readonly IValidator<UpdateProductRequest> _validator;
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            IValidator<UpdateProductRequest> validator,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _validator = validator;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.product, cancellationToken);

            _ = await _categoryRepository.GetAsync(request.product.CategoryId, cancellationToken) ?? throw new CategoryNotFoundException();
            _ = await _productRepository.GetAsync(request.product.Id, cancellationToken) ?? throw new ProductNotFoundException();

            return await _productRepository.UpdateAsync(_mapper.Map<Product>(request.product), cancellationToken);
        }
    }
}
