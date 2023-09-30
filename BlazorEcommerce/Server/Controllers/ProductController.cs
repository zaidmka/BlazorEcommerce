using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("search/{searchText}/{page}")]

        public async Task<ActionResult<ServiceResponse<ProductSearchResult>>> SearchProducts(string searchText,int page = 1)
        {
            var result = await _productServicecs.SearchProducts(searchText,page);
            return Ok(result);
        }
        [HttpGet("searchsuggestions/{searchText}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchsuggestions(string searchText)
        {
            var result = await _productServicecs.GetProductSearchsuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await _productServicecs.GetFeaturedProducts();
            return Ok(result);
        }

        [HttpGet("admin") ,Authorize(Roles ="Admin")]
                public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            var result = await _productServicecs.GetAdminProducts();
            return Ok(result);
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var result = await _productServicecs.CreateProduct(product);
            return Ok(result);
        }
        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
        {
            var result = await _productServicecs.UpdateProduct(product);
            return Ok(result);
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var result = await _productServicecs.DeleteProduct(id);
            return Ok(result);
        }


    }
}
