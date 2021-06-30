using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.UseCases.Orders.Commands.CheckoutOrder;
using Ordering.Application.UseCases.Orders.Commands.DeleteOrder;
using Ordering.Application.UseCases.Orders.Commands.UpdateOrder;
using Ordering.Application.UseCases.Orders.Queries.GetOrderList;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{UserName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);

            return orders;
        }

        [HttpGet(Name = "CheckoutOrder")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand {Id = id};
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
