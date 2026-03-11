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
		Task<Category?> GetCategoryBySlugAsync(string slug);
		Task<Product?> GetProductBySlugAsync(string slug);
		Task<bool> AddProductAsync(Product product);
	}
}
