namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductServicecs
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
    }
}
