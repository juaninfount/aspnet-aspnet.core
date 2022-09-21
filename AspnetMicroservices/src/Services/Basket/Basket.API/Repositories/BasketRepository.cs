using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            this._redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        public async Task DeleteBasket(string username)
        {
            await _redisCache.RemoveAsync(username);                            
        }

        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);
            if(string.IsNullOrEmpty(basket))
                return null;
        
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
    }
}