using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
