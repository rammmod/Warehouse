using WarehouseApi.Commands.Orders;
using WarehouseApi.Models.Order;
using WarehouseApi.Queries.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WarehouseApi.Controllers
{
    public class OrderController : DefaultController
    {
        private readonly ISender _sender;

        public OrderController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var orders = await _sender.Send(new GetOrdersQuery());

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest order)
        {
            var newOrder = await _sender.Send(new CreateOrderCommand(order));

            return Ok(newOrder);
        }
    }
}