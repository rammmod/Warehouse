using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        readonly IValidator<CreateCategoryRequest> _validator;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public CreateCategoryCommandHandler(
            IValidator<CreateCategoryRequest> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.category, cancellationToken);

            return (await _categoryRepository.AddAsync(_mapper.Map<Category>(request.category), cancellationToken)).Id;
        }
    }
}
