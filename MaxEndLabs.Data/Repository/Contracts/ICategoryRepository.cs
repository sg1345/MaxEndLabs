using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<Category?> GetCategoryBySlugAsync(string slug);
		Task<string?> GetCategorySlugAsync(int id);
	}
}
