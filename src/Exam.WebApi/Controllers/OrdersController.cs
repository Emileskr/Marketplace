using Application.Orders.Commands;
using Application.Orders.Queries;
using Domain.Models;
using Domain.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.WebApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Updates the status of the order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="updateOrderStatus"></param>
        /// <returns></returns>
        /// <response code="204">Request successfuly achieved</response>
        /// <response code="404">Order not found</response>
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody]UpdateOrderStatusDto updateOrderStatus)
        {
            await _mediator.Send(new UpdateOrderStatusCommand
            {
                OrderId = orderId,
                Status = updateOrderStatus.Status
            });
            return NoContent();
        }
        /// <summary>
        /// Creates new order
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        /// <response code="204">Request successfuly achieved</response>
        /// <response code="404">User not found</response>
        /// <response code="404">Item not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Create([FromBody]CreateOrderCommand newOrder)
        {
            await _mediator.Send(newOrder);
            return Created();
        }
        /// <summary>
        /// Gets all orders by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of orders</returns>
        /// <response code="200">Request successfuly achieved</response>
        [HttpGet("/ordersFromUser")]
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId([FromQuery]int userId)
        {
            return Ok(await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId }));
        }
        /// <summary>
        /// Gets order by it's Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>orders</returns>
        /// <response code="200">Request successfuly achieved</response>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            return Ok(await _mediator.Send(new GetOrderByIdQuery { Id = orderId}));
        }
    }
}
