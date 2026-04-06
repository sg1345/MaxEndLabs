using MaxEndLabs.Service.Models.ShoppingCart;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IShoppingCartService
	{
		Task<ShoppingCartIndexDto> GetAllCartItemsAsync(Guid userId);
		Task AddProductToShoppingCartAsync(CartItemCreateDto dto);
		Task RemoveCartItemFromShoppingCartAsync(CartItemDeleteDto dto);
		Task DeleteAllCartItemsFromShoppingCartAsync(Guid cartId);
		Task<Guid> GetOrCreateShoppingCartAsync(Guid userId);
	}
}
