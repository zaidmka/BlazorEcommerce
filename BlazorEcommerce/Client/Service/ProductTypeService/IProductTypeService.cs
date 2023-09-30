namespace BlazorEcommerce.Client.Service.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;
        public List<ProductType> ProductTypes { get; set; }

        Task GetProductTypes();

        Task AddProducttype(ProductType productType);
        Task UpdateProducttype(ProductType productType);

        ProductType CreateNewProductType();

    }
}
