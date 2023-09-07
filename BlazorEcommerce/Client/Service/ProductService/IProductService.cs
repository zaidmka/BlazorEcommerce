using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Service.ProductService
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task GetProducts();
    }
}
