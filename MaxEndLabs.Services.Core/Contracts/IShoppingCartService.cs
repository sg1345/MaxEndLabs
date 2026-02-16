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
		Task<ShoppingCartIndexViewModel> GetAllCartItemsAsync(string userId);
		Task AddProductToShoppingCartAsync(CartItemCreateViewModel model, int cartId);
		Task RemoveCartItemFromShoppingCartAsync(CartItemRemoveViewModel model);
		Task DeleteAllCartItemsFromShoppingCartAsync(int cartId);
		Task<int> GetShoppingCartIdAsync(string userId);
		Task<int> CreateShoppingCartAsync(string userId);
	}
}
