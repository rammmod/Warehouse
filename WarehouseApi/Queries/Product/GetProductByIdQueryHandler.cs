using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.DTOs;
using WarehouseApi.Exceptions;
using MediatR;

namespace WarehouseApi.Queries.Product
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        readonly IValidator<int> _validator;
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;

        public GetProductByIdQueryHandler(
            IValidator<int> validator,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _validator = validator;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.Id, cancellationToken);

            var product = _mapper.Map<ProductDTO>(await _productRepository.GetAsync(request.Id, cancellationToken));

            return product ?? throw new ProductNotFoundException();
        }
    }
}
