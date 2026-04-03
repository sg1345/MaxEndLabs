using System.Text.RegularExpressions;
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

        public async Task<IEnumerable<Product>?> GetSearchProductsAsync(string? searchTerm, int skip, int take)
        {
            IQueryable<Product> query = DbContext.Products
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(p=> p.Category)
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.UpdatedAt);

            if ( searchTerm != null)
            {
                return await query
                    .Where(p=>p.Name.Contains(searchTerm))
                    .Skip(skip)
                    .Take(take)
                    .ToArrayAsync();
            }
            else if (string.IsNullOrEmpty(searchTerm))
            {
                return await query
                    .Skip(skip)
                    .Take(take)
                    .ToArrayAsync();
            }

            return null;
        }

        public async Task<int> GetCountAsync(string? searchTerm)
        {
            IQueryable<Product> query = DbContext.Products
                .AsNoTracking()
                .IgnoreQueryFilters();

            if (searchTerm != null)
            {
                return await query
                    .CountAsync(p => p.Name.Contains(searchTerm));
            }
            else if (string.IsNullOrEmpty(searchTerm))
            {
                return await query
                    .CountAsync();
            }

            return 0;
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

		public async Task<Product?> GetProductAsync(string slug, bool isFiltered, bool includeCategory, bool includeVariants)
		{
			IQueryable<Product> query = DbContext.Products
				.AsNoTracking();

            if (!isFiltered)
            {
	            query = query.IgnoreQueryFilters();
            }

            if (includeCategory)
            {
	            query = query.Include(p => p.Category);

            }

            if (includeVariants)
            {
	            query = query.Include(p => p.ProductVariants);

            }

			return await query
                .SingleOrDefaultAsync(p => p.Slug == slug);
        }

		public async Task<Product?> GetProductAsync(int id)
		{
			return await DbContext.Products
				.Include(p => p.Category)
				.SingleOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddProductAsync(Product product)
		{
			await DbContext!.Products.AddAsync(product);
		}

		public async Task<IEnumerable<ProductVariant>> GetProductVariantsByProductIdAsync(int productId)
		{
			return await DbContext.ProductVariants
				.IgnoreQueryFilters()
				.Where(pv=> pv.ProductId == productId)
				.ToListAsync();
		}

		public void RemoveRangeProductVariantAsync(IEnumerable<ProductVariant> productVariants)
		{
			bool changesMade = false;
			foreach (var productVariant in productVariants)
			{
				if (productVariant.IsDeleted == false)
				{
					productVariant.IsDeleted = true;
					changesMade = true;
				}
			}

			if(changesMade)
				DbContext!.UpdateRange(productVariants);

		}

		public async Task AddProductVariantAsync(ProductVariant productVariant)
		{
			await DbContext!.ProductVariants.AddAsync(productVariant);
		}

		public void SoftDeleteProduct(Product product)
		{
			if (product.IsPublished)
			{
				product.IsPublished = false;
				product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
				product.Slug = $"{product.Slug}-{DateTime.UtcNow:yyyyMMdd-HHmmss}";

				foreach (var productVariant in product.ProductVariants)
				{
					productVariant.IsDeleted = true;
				}

				DbContext!.Products.Update(product);
			}
		}

		public void RestoreProduct(Product product)
		{
			string pattern = @"-\d{8}-\d{6}$";

			product.IsPublished = true;
			product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
			product.Slug = Regex.Replace(product.Slug, pattern, string.Empty);

			foreach (var productVariant in product.ProductVariants)
			{
				productVariant.IsDeleted = false;
			}

			DbContext!.Products.Update(product);
			DbContext!.ProductVariants.UpdateRange(product.ProductVariants);
		}

		public void ProductUpdate(Product product)
		{
			DbContext!.Products.Update(product);
		}
	}
}
