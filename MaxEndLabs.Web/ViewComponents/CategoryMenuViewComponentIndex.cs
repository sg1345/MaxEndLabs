using MaxEndLabs.Data.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.ViewComponents
{
	public class CategoryMenuViewComponent : ViewComponent
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _categoryRepository.GetAllCategoriesAsync();
			return View(categories);
		}
	}
}
