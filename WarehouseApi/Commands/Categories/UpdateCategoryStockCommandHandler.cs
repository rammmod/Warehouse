using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Category;
using WarehouseApi.Services;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public class UpdateCategoryStockCommandHandler : IRequestHandler<UpdateCategoryStockCommand, bool>
    {
        readonly IValidator<UpdateCategoryStockRequest> _validator;
        readonly ICategoryRepository _categoryRepository;
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;
        readonly IStockService _stockService;

        public UpdateCategoryStockCommandHandler(
            IValidator<UpdateCategoryStockRequest> validator,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IMapper mapper,
            IStockService stockService)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _stockService = stockService;
        }

        public async Task<bool> Handle(UpdateCategoryStockCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.category, cancellationToken);

            var categoryBeforeChange = await _categoryRepository.GetAsync(request.category.Id, cancellationToken) ?? throw new CategoryNotFoundException();

            var isCategoryUpdated = await _categoryRepository.UpdateStockAsync(_mapper.Map<Category>(request.category), cancellationToken);

            if (isCategoryUpdated && categoryBeforeChange.OutOfStock != request.category.OutOfStock)
            {
                var products = (await _productRepository.GetAllAsync(cancellationToken)).Where(m => m.CategoryId == request.category.Id);

                foreach (var product in products)
                {
                    await _stockService.CheckForStockChange(product, cancellationToken);
                }
            }

            return isCategoryUpdated;
        }
    }
}
