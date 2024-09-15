using AutoMapper;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Product
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IList<ProductDTO>>
    {
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;

        public GetProductsQueryHandler(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IList<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IList<ProductDTO>>(await _productRepository.GetAllAsync(cancellationToken));
        }
    }
}
