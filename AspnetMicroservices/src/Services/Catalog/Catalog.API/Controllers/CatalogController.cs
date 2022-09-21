using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Catalog.API.Repositories;
using Catalog.API.Entities;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null) 
            {
                _logger.LogError($"Product with Id {id} not found");
                return NotFound();
            }
            return Ok(product);
        }
        
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category) 
        {
            var product = await _repository.GetProductByCategory(category);
            if (product == null)
            {
                _logger.LogError($"Product with Category {category} not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
            await _repository.CreateProduct(product);
            return Ok(_repository.GetProduct(product.Id));
            //return CreatedAtRoute("GetProduct", new { id=product.Id}, product); // busca la ruta con nombre Name = "GetProduct"
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product) 
        {
            await _repository.UpdateProduct(product);
            return Ok(_repository.GetProduct(product.Id));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] // el tipo retornado
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {            
            return Ok(await _repository.DeleteProduct(id));
        }
    }
}