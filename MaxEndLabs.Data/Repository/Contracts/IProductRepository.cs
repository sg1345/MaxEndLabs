using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IProductRepository
	{
		Task<bool> SlugExistsAsync(string slug, int productId);
		Task<bool> SlugExistsAsync(string slug);
		Task<IEnumerable<Product>> GetAllProductsAsync();
		Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
		Task<Product?> GetProductAsync(string slug);
		Task<Product?> GetProductAsync(int id);
		Task AddProductAsync(Product product);
		Task<int> SaveChangesAsync();
		Task<IEnumerable<ProductVariant>> GetProductVariantsByProductIdAsync(int productId);
		void RemoveRangeProductVariantAsync(IEnumerable<ProductVariant> productVariants);
		Task AddProductVariantAsync (ProductVariant productVariant);
		void SoftDeleteProduct(Product product);
	}
}
