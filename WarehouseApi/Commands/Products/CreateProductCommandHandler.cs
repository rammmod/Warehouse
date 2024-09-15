using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Product;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        readonly IValidator<CreateProductRequest> _validator;
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public CreateProductCommandHandler(
            IValidator<CreateProductRequest> validator,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _validator = validator;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.product, cancellationToken);

            _ = await _categoryRepository.GetAsync(request.product.CategoryId, cancellationToken) ?? throw new CategoryNotFoundException();

            return (await _productRepository.AddAsync(_mapper.Map<Product>(request.product), cancellationToken)).Id;
        }
    }
}
