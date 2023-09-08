using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServicecs _productServicecs;

        public ProductController(IProductServicecs productServicecs)
        {  
            _productServicecs = productServicecs;
        }

        public IProductServicecs ProductServicecs { get; }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var result = await _productServicecs.GetProductsAsync();
            return Ok(result);//zaid1
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var result = await _productServicecs.GetProductAsync(productId);
            return Ok(result);//zaid1
        }

        [HttpGet("category/{categoryUrl}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>>GetProductByCategory(string categoryUrl)
        {
            var result = await _productServicecs.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{searchText}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchProducts(string searchText)
        {
            var result = await _productServicecs.SearchProducts(searchText);
            return Ok(result);
        }
        [HttpGet("searchsuggestions/{searchText}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchsuggestions(string searchText)
        {
            var result = await _productServicecs.GetProductSearchsuggestions(searchText);
            return Ok(result);
        }
    } 
}
