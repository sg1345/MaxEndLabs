using MaxEndLabs.Service.Models.Product;
using MaxEndLabs.ViewModels;
using MaxEndLabs.ViewModels.Product;

namespace MaxEndLabs.Services.Core.Contracts
{
    public interface IProductService
	{
		Task<bool> ProductExistsAsync(string productName, int productId);
		Task<bool> ProductExistsAsync(string productName);
		Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
		Task<ProductsPageDto> GetAllProductsAsync();
		Task<ProductsPageDto> GetProductsByCategoryAsync(string categorySlug);
		Task<ProductDetailsDto> GetProductDetailsAsync(string productSlug);
        Task<ProductFormDto> GetProductCreateViewModelAsync();
        Task<string> AddProductAsync(ProductCreateDto model);
        Task<ManageVariantsViewModel> GetProductAsync(string productSlug);
        Task ManageProductVariantsAsync(ManageVariantsViewModel model);
        Task<ProductFormViewModel> GetProductEditViewModelAsync(string productSlug);
        Task<ProductFormViewModel> GetProductEditViewModelAsync(int productId);
		Task<(string categorySlug, string productSlug)> EditProductAsync(ProductFormViewModel model);
		Task DeleteProductAsync(string productSlug);
	}
}
