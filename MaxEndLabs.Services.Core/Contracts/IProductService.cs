using MaxEndLabs.ViewModels;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IProductService
	{
		Task<bool> ProductExistsAsync(string productName, int productId);
		Task<bool> ProductExistsAsync(string productName);
		Task<IEnumerable<ProductIndexViewModel>> GetAllCategoriesAsync();
		Task<ProductsPageViewModel> GetAllProductsAsync();
		Task<ProductsPageViewModel> GetProductsByCategoryAsync(string categorySlug);
		Task<ProductDetailsViewModel> GetProductDetailsAsync(string categorySlug, string productSlug);
        Task<ProductFormViewModel> GetProductCreateViewModelAsync();
        Task<string> AddProductAsync(ProductFormViewModel model);
        Task<ManageVariantsViewModel> GetProductAsync(string productSlug);
        Task ManageProductVariantsAsync(ManageVariantsViewModel model);
        Task<ProductFormViewModel> GetProductEditViewModelAsync(string productSlug);
        Task<ProductFormViewModel> GetProductEditViewModelAsync(int productId);
		Task<(string categorySlug, string productSlug)> EditProductAsync(ProductFormViewModel model);
		Task DeleteProductAsync(string productSlug);
	}
}
