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

		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			return await DbContext.Products
				.Include(p=>p.Category)
				.AsNoTracking()
				.ToArrayAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
		{
			return await DbContext.Products
				.Include(p => p.Category)
				.AsNoTracking()
				.Where(p=> p.CategoryId == categoryId)
				.ToArrayAsync();
		}

		public async Task<Product?> GetProductAsync(string slug)
		{
			return await DbContext.Products
				.AsNoTracking()
				.Include(p => p.Category)
				.Include(p => p.ProductVariants)
				.FirstOrDefaultAsync(p => p.Slug == slug);
		}

		public async Task<Product?> GetProductAsync(int id)
		{
			return await DbContext.Products
				.AsNoTracking()
				.Include(p => p.Category)
				.Include(p => p.ProductVariants)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddProductAsync(Product product)
		{
			await DbContext!.Products.AddAsync(product);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await DbContext!.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProductVariant>> GetProductVariantsByProductIdAsync(int productId)
		{
			return await DbContext.ProductVariants
				.Where(pv=> pv.ProductId == productId)
				.ToListAsync();
		}

		public void RemoveRangeProductVariantAsync(IEnumerable<ProductVariant> productVariants)
		{
			DbContext!.RemoveRange(productVariants);
		}

		public async Task AddProductVariantAsync(ProductVariant productVariant)
		{
			await DbContext!.ProductVariants.AddAsync(productVariant);
		}

		public void SoftDeleteProduct(Product product)
		{
				product.IsPublished = false;
				product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
				product.Slug = $"{product.Slug}-{DateTime.UtcNow:yyyyMMdd-HHmmss}";

				DbContext!.Products.Update(product);
		}
	}
}
