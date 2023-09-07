using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductServicecs
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                response.Success=false;
                response.Message = "Sorry, But this Product does not EXITS!";
            }
            else
            {
                response.Success=true;
                response.Data = product;
                response.Message="DONE! YeeeeAAAAAAHHHHH";
            }
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products.ToListAsync()
            };
            return response;
        }
    }
}
