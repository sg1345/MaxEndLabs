using System.Diagnostics.CodeAnalysis;
using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<Category?> GetCategoryAsync(string slug);
        Task<Category?> GetCategoryAsync(Guid id);
    }
}
