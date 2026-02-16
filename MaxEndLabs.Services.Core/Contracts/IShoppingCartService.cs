using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.ViewModels;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IShoppingCartService
	{
		Task<IEnumerable<ShoppingCartIndexViewModel>> GetShoppingCartAsync();
		Task AddProductToShoppingCartAsync();
		Task RemoveProductFromShoppingCartAsync();
		Task DeleteAllProductsFromShoppingCartAsync();
	}
}
