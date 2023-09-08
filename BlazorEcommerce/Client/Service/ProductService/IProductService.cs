
namespace BlazorEcommerce.Client.Service.ProductService
{
    public interface IProductService
    {
        event Action ProductsChange;
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
    }
}
