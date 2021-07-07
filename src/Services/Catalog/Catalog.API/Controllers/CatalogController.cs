using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.Get();
            return Ok(products);
        }


        [HttpPut]
        [Route("products")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
                await _repository.Update(product);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }


        [HttpPost]
        [Route("products")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpDelete]
        [Route("products/{id:length(24)}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _repository.Delete(id);
            return Ok();
        }

        /**/

        [HttpGet]
        [Route("products/{id:length(24)}")]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.Get(id);
            if (product == null)
            {
                _logger.LogError($"couldn't find a product with id = {id}");
                return NotFound();
            }

            return Ok(product);
        }


        /**/
        [HttpGet]
        [Route("categories/{name}/products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string name)
        {
            var products = await _repository.GetByCategory(name);
            return Ok(products);
        }


    }
}
