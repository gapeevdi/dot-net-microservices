using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Discount;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IDiscountClient _discountClient;

        public BasketController(IBasketRepository basketRepository, IDiscountClient discountClient)
        {
            _repository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountClient = discountClient ?? throw new ArgumentNullException(nameof(discountClient));
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
    }
}
