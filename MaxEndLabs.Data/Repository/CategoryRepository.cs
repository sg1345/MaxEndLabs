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

		public async Task<Category?> GetCategoryAsync(string slug)
		{
			return await DbContext.Categories
				.SingleOrDefaultAsync(c => c.Slug == slug);
		}

        public async Task<Category?> GetCategoryAsync(Guid id)
        {
            return await DbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
