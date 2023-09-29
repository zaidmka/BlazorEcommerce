namespace BlazorEcommerce.Client.Service.CategoryService
{
    public interface ICategoryService
    {

        event Action OnChange;

        List<Category> Categories { get; set; }

        List<Category> AdminCategories { get; set; }


        Task GetCategories();
        Task GetAdminCategories();
        Task DeleteCategory(int categoryId);
        Task UpdateCategory(Category category);
        Task AddCategory(Category category);
        Category CreateNewCategory();

    }
}
