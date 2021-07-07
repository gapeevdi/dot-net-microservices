using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Discount;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IDiscountClient _discountClient;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository, IDiscountClient discountClient, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountClient = discountClient ?? throw new ArgumentNullException(nameof(discountClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));

        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Get(string userName)
        {
            var basket = await _repository.Get(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Update([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountClient.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            await _repository.Update(basket);
            return Ok();
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(string userName)
        {
            await _repository.Delete(userName);
            return Ok();
        }

        
        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.Get(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            await _repository.Delete(basket.UserName);

            return Accepted();
        }
    }
}
 