using Microsoft.AspNetCore.Mvc;
using MarketApi.Services;
using MarketApi.Models;

namespace MarketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        //No Async suffix
        public async Task<IActionResult> Create(Product product)
        {
            if (await _productService.IsExistAsync(product.Id))
            {
                return BadRequest("Product with this ID already exist");
            }

            var createdProduct = await _productService.CreateAsync(product);

            return CreatedAtAction("Create", createdProduct);
        }

        [HttpGet]
        //No Async suffix
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        [HttpGet]
        //No Async suffix
        public async Task<IActionResult> Get(int id)
        {
            if (!await _productService.IsExistAsync(id))
            {
                return BadRequest("Product not found");
            }

            var products = await _productService.GetAsync(id);

            return Ok(products);
        }

        [HttpPut]
        //No Async suffix
        public async Task<IActionResult> Update(Product product)
        {
            if (!await _productService.IsExistAsync(product.Id))
            {
                return base.BadRequest("Product not found");
            }

            var updatedProduct = await _productService.UpdateAsync(product);

            return base.Ok(updatedProduct);
        }

        [HttpDelete]
        //No Async suffix
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _productService.IsExistAsync(id))
            {
                return BadRequest("Product with this ID does not exist");
            }

            var product = await _productService.DeleteAsync(id);

            return Ok(product);
        }
    }
}
