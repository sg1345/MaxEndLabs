using Microsoft.EntityFrameworkCore;

using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;

namespace MaxEndLabs.Data.Repository
{
	public class CategoryRepository : BaseRepository, ICategoryRepository
	{
		public CategoryRepository(MaxEndLabsDbContext dbContext) 
			: base(dbContext)
		{
		}

		public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			return await DbContext.Categories
				.AsNoTracking()
				.ToArrayAsync();
		}

		public async Task<Category?> GetCategoryBySlugAsync(string slug)
		{
			return await DbContext.Categories
				.FirstOrDefaultAsync(c => c.Slug == slug);
		}

		public async Task<string?> GetCategorySlugAsync(int id)
		{
			return await DbContext.Categories
				.Where(c => c.Id == id)
				.Select(c => c.Slug)
				.FirstOrDefaultAsync();
		}
	}
}
