using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Product;
using WarehouseApi.Services;
using MediatR;

namespace WarehouseApi.Commands.Products
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, bool>
    {
        readonly IValidator<UpdateProductStockRequest> _validator;
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;
        readonly IStockService _stockService;

        public UpdateProductStockCommandHandler(
            IValidator<UpdateProductStockRequest> validator,
            IProductRepository productRepository,
            IMapper mapper,
            IStockService stockService)
        {
            _validator = validator;
            _productRepository = productRepository;
            _mapper = mapper;
            _stockService = stockService;
        }

        public async Task<bool> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.product, cancellationToken);

            var product = await _productRepository.GetAsync(request.product.Id, cancellationToken) ?? throw new ProductNotFoundException();

            product.Stock = request.product.Stock;

            return await _stockService.UpdateStockAndCheckForChange(product, cancellationToken);
        }
    }
}
