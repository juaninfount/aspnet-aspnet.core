using Basket.API.Repositories;
using Basket.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;


namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketRepository _IBasketRepository;
        public BasketController(IBasketRepository repository, ILogger<BasketController> logger)
        {
            this._IBasketRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{userName}")] //, Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _IBasketRepository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }


        [HttpDelete("{username}")] // ,Name = "DeleteBasket"
        [ProducesResponseType(typeof(Entities.ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string username)
        {            
            await this._IBasketRepository.DeleteBasket(username);
            return Ok("Item removed");
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entities.ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody]Entities.ShoppingCart basket)
        {            
            return Ok(await this._IBasketRepository.UpdateBasket(basket)) ;
        }
    }
}