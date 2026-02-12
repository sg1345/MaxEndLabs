using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Services.Core
{
	public class ProductService : IProductService
	{
		private readonly MaxEndLabsDbContext _context;

		public ProductService(MaxEndLabsDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ProductIndexViewModel>> GetAllCategoriesAsync()
		{
			return await _context.Categories
				.AsNoTracking()
				.OrderByDescending(c => c.Name)
				.Select(c => new ProductIndexViewModel
				{
					Id = c.Id,
					Name = c.Name,
					Slug = c.Slug
				})
				.ToListAsync();
		}

		public async Task<ProductsPageViewModel> GetAllProductsAsync()
		{
			var products = await _context.Products
				.AsNoTracking()
				.OrderBy(p => p.Name)
				.Select(p => new ProductListViewModel()
				{
					Id = p.Id,
					Name = p.Name,
					Slug = p.Slug,
					Price = p.Price,
					MainImageUrl = p.MainImageUrl,
					CategorySlug = p.Category.Slug
				})
				.ToListAsync();

			return new ProductsPageViewModel
			{
				Title = "All Products",
				Products = products
			};
		}

		public async Task<ProductsPageViewModel> GetProductsByCategoryAsync(string categorySlug)
		{
			var category = await _context.Categories
				.FirstOrDefaultAsync(c => c.Slug == categorySlug);

			if (category == null)
				return null;

			var products = await _context.Products
				.AsNoTracking()
				.Where(p => p.CategoryId == category.Id)
				.OrderBy(p => p.Name)
				.Select(p => new ProductListViewModel()
				{
					Id = p.Id,
					Name = p.Name,
					Slug = p.Slug,
					Price = p.Price,
					MainImageUrl = p.MainImageUrl,
					CategorySlug = p.Category.Slug
				})
				.ToListAsync();

			return new ProductsPageViewModel
			{
				Title = category.Name,
				Products = products
			};
		}

		public async Task<ProductDetailsViewModel> GetProductDetailsAsync(string categorySlug, string productSlug)
		{
			var product = await _context.Products
				.AsNoTracking()
				.Include(p => p.Category)
				.Include(p => p.ProductVariants)
				.FirstOrDefaultAsync(p => p.Slug == productSlug && p.Category.Slug == categorySlug);

			if (product == null)
				return null;

			var productVariant = await _context.ProductVariants
				.AsNoTracking()
				.Where(pv => pv.ProductId == product.Id)
				.Select(pv => new ProductVariantViewModel()
				{
					Id = pv.Id,
					VariantName = pv.VariantName,
					Price = pv.Price ?? product.Price
				})
				.ToArrayAsync();

			return new ProductDetailsViewModel
			{
				Id = product.Id,
				Name = product.Name,
				ProductSlug = product.Slug,
				CategorySlug = product.Category.Slug,
				Description = product.Description,
				Price = product.Price,
				MainImageUrl = product.MainImageUrl,
				ProductVariants = productVariant
			};
		}
	}
}
