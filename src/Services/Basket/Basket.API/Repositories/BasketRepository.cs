using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _cache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<ShoppingCart> Get(string userName)
        {
            var jsonFormattedCart = await _cache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(jsonFormattedCart))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(jsonFormattedCart);
        }

        public async Task Update(ShoppingCart cart)
        {
            var jsonFormattedCart = JsonConvert.SerializeObject(cart);
            await _cache.SetStringAsync(jsonFormattedCart, cart.UserName);
        }

        public Task Delete(string userName) => _cache.RemoveAsync(userName);
    }
}
