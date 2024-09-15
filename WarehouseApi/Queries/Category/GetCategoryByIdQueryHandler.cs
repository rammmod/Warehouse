using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.DTOs;
using WarehouseApi.Exceptions;
using MediatR;

namespace WarehouseApi.Queries.Category
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDTO>
    {
        readonly IValidator<int> _validator;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(
            IValidator<int> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.Id, cancellationToken);

            var category = _mapper.Map<CategoryDTO>(await _categoryRepository.GetAsync(request.Id, cancellationToken));

            return category ?? throw new CategoryNotFoundException();
        }
    }
}
