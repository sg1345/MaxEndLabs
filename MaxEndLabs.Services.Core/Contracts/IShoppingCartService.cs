using MaxEndLabs.Service.Models.ShoppingCart;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IShoppingCartService
	{
		Task<ShoppingCartIndexDto> GetAllCartItemsAsync(string userId);
		Task AddProductToShoppingCartAsync(CartItemCreateDto dto);
		Task RemoveCartItemFromShoppingCartAsync(CartItemDeleteDto dto);
		Task DeleteAllCartItemsFromShoppingCartAsync(int cartId);
		Task<int> GetOrCreateShoppingCartAsync(string userId);
	}
}
