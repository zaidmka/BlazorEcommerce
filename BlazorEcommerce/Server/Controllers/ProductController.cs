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
    } 
}
