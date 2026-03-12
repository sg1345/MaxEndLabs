using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IProductRepository
	{
		Task<bool> SlugExistsAsync(string slug, int productId);
		Task<bool> SlugExistsAsync(string slug);
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<IEnumerable<Product>> GetAllProductsAsync();
		Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
		Task<Category?> GetCategoryBySlugAsync(string slug);
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
