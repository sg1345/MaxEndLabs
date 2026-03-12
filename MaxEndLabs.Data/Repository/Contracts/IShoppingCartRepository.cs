using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IShoppingCartRepository
	{
		Task AddToCartAsync(int userId, int productVariantId, int quantity);
		Task RemoveFromCartAsync(int userId, int productVariantId);
		Task UpdateCartItemAsync(int userId, int productVariantId, int quantity);
		Task ClearCartAsync(int userId);
		Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);
		Task<IEnumerable<CartItem>> GetCartItemsAsync(string productSlug);
		void CartItemsRemoveRange(IEnumerable<CartItem> cartItems);
	}
}
