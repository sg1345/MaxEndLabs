using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.ViewModels;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IProductService
	{
		Task<IEnumerable<ProductIndexViewModel>> GetAllCategoriesAsync();
		Task<ProductsPageViewModel> GetAllProductsAsync();
		Task<ProductsPageViewModel> GetProductsByCategoryAsync(string categorySlug);
	}
}
