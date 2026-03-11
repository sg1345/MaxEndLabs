using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Data.Repository
{
	public class ProductRepository : BaseRepository, IProductRepository
	{
		public ProductRepository(MaxEndLabsDbContext dbContext) 
			: base(dbContext)
		{
		}

		public async Task<bool> SlugExistsAsync(string slug, int productId)
		{
			return await DbContext.Products
				.AnyAsync(p => p.Slug == slug && p.Id != productId);
		}

		public async Task<bool> SlugExistsAsync(string slug)
		{
			return await DbContext.Products
				.AnyAsync(p => p.Slug == slug);
		}

		public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			return await DbContext.Categories
				.AsNoTracking()
				.ToArrayAsync();
		}

		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			return await DbContext.Products
				.Include(p=>p.Category)
				.AsNoTracking()
				.ToArrayAsync();
		}

		public async Task<Category?> GetCategoryBySlugAsync(string slug)
		{
			return (await DbContext.Categories
				.FirstOrDefaultAsync(c => c.Slug == slug));
		}

		public async Task<Product?> GetProductBySlugAsync(string slug)
		{
			return await DbContext.Products
				.AsNoTracking()
				.Include(p => p.Category)
				.Include(p => p.ProductVariants)
				.FirstOrDefaultAsync(p => p.Slug == slug);
		}

		public async Task<bool> AddProductAsync(Product product)
		{
			await DbContext!.Products.AddAsync(product);
			int resultCount = await SaveChangesAsync();

			return resultCount == 1;
		}
	}
}
