using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;

namespace MaxEndLabs.Services.Core.Contracts
{
    public interface IProductService
	{
		Task<bool> ProductExistsAsync(string productName, int productId);
		Task<bool> ProductExistsAsync(string productName);
        Task<ProductPaginationDto> GetProductSearchAsync(string searchTerm, int page, int pageSize);
		Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
		Task<ProductsPageDto> GetAllProductsAsync();
		Task<ProductsPageDto> GetProductsByCategoryAsync(string categorySlug);
		Task<ProductDetailsDto> GetProductDetailsAsync(string productSlug, bool isFiltered);
        Task<ProductFormDto> GetProductCreateDtoAsync();
        Task<string> AddProductAsync(ProductCreateDto dto);
        Task<ProductVariantListDto> GetProductAsync(string productSlug, bool isFiltered);
        Task ManageProductVariantsAsync(ProductVariantListDto dto);
        Task<ProductFormDto> GetProductEditDtoAsync(string productSlug);
		Task<(string categorySlug, string productSlug)> EditProductAsync(ProductFormDto dto);
		Task SoftDeleteProductAsync(string productSlug);
		Task RestoreProductAsync(string productSlug);
	}
}
