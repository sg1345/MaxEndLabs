using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IProductRepository
	{
		Task<bool> SlugExistsAsync(string slug, Guid productId);
		Task<bool> SlugExistsAsync(string slug);
		Task<IEnumerable<Product>?> GetSearchProductsAsync(string? searchTerm, int skip, int take);
        Task<int> GetCountAsync(string? searchTerm);
        Task<IEnumerable<Product>> GetAllProductsAsync();
		Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
		Task<Product?> GetProductAsync(string slug, bool isFiltered, bool includeCategory, bool includeVariants);
		Task<Product?> GetProductAsync(Guid id);
		Task AddProductAsync(Product product);
		Task<int> SaveChangesAsync();
		Task<IEnumerable<ProductVariant>> GetProductVariantsByProductIdAsync(Guid productId);
		void UpdateRangeProductVariantAsync(IEnumerable<ProductVariant> productVariants);
		Task AddProductVariantAsync (ProductVariant productVariant);
		void RestoreProduct(Product product);
		void ProductUpdate(Product product);
	}
}
