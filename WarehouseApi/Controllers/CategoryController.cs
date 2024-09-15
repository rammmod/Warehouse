using WarehouseApi.Commands.Categories;
using WarehouseApi.Models.Category;
using WarehouseApi.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseApi.Controllers
{
    public class CategoryController : DefaultController
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var categories = await _sender.Send(new GetCategoriesQuery());

            return Ok(categories);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int Id)
        {
            var category = await _sender.Send(new GetCategoryByIdQuery(Id));

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] CreateCategoryRequest category)
        {
            var newCategory = await _sender.Send(new CreateCategoryCommand(category));

            return Ok(newCategory);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest category)
        {
            var isUpdated = await _sender.Send(new UpdateCategoryCommand(category));

            return Ok(isUpdated);
        }

        [HttpPut(nameof(UpdateStock))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateCategoryStockRequest category)
        {
            var isStockUpdated = await _sender.Send(new UpdateCategoryStockCommand(category));

            return Ok(isStockUpdated);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryRequest category)
        {
            var isDeleted = await _sender.Send(new DeleteCategoryCommand(category));

            return Ok(isDeleted);
        }
    }
}