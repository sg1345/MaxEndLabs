using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.ViewComponents
{
	public class CategoryMenuViewComponent : ViewComponent
	{
		private readonly IProductService _productService;

		public CategoryMenuViewComponent(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categoriesDto = await _productService.GetAllCategoriesAsync();

			var model = categoriesDto.Select(c => new ProductIndexViewModel
				{
					Id = c.Id,
					Slug = c.Slug,
					Name = c.Name
					
				})
				.ToList();

			return View(model);
		}
	}
}
