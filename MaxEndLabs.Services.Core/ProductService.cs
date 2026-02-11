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
					CategoryId = p.CategoryId,
					Price = p.Price,
					MainImageUrl = p.MainImageUrl
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
					CategoryId = p.CategoryId,
					Price = p.Price,
					MainImageUrl = p.MainImageUrl
				})
				.ToListAsync();

			return new ProductsPageViewModel
			{
				Title = category.Name,
				Products = products
			};
		}
	}
}
