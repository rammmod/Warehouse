using AutoMapper;
using FluentValidation;
using WarehouseApi.Domain.Abstractions;
using WarehouseApi.Domain.Entities;
using WarehouseApi.Exceptions;
using WarehouseApi.Models.Category;
using MediatR;

namespace WarehouseApi.Commands.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        readonly IValidator<UpdateCategoryRequest> _validator;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(
            IValidator<UpdateCategoryRequest> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request.category, cancellationToken);

            _ = await _categoryRepository.GetAsync(request.category.Id, cancellationToken) ?? throw new CategoryNotFoundException();

            return await _categoryRepository.UpdateAsync(_mapper.Map<Category>(request.category), cancellationToken);
        }
    }
}
