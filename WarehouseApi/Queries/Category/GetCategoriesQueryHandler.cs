using AutoMapper;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.DTOs;
using MediatR;

namespace WarehouseApi.Queries.Category
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IList<CategoryDTO>>
    {
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public GetCategoriesQueryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IList<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IList<CategoryDTO>>(await _categoryRepository.GetAllAsync(cancellationToken));
        }
    }
}
