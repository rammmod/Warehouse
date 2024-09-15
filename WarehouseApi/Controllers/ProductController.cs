using WarehouseApi.Commands.Products;
using WarehouseApi.Models.Product;
using WarehouseApi.Queries.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseApi.Controllers
{
    public class ProductController : DefaultController
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var products = await _sender.Send(new GetProductsQuery());

            return Ok(products);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await _sender.Send(new GetProductByIdQuery(Id));

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] CreateProductRequest product)
        {
            var newProduct = await _sender.Send(new CreateProductCommand(product));

            return Ok(newProduct);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest product)
        {
            var isUpdated = await _sender.Send(new UpdateProductCommand(product));

            return Ok(isUpdated);
        }

        [HttpPut(nameof(UpdateStock))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateProductStockRequest product)
        {
            var isStockUpdated = await _sender.Send(new UpdateProductStockCommand(product));

            return Ok(isStockUpdated);
        }
    }
}