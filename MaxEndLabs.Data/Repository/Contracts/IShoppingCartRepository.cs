using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IShoppingCartRepository
	{
		Task<int> GetShoppingCartIdAsync(string userId);
		Task AddToCartAsync(CartItem cartItem);
		void SoftDeleteFromCartAsync(CartItem cartItem);
		Task<CartItem?> GetCartItemIgnoreFilterAsync(int cartId, int productId, int productVariantId);
		Task<CartItem?> GetCartItemAsync(int cartId, int productId, int productVariantId);
		void ClearCart(IEnumerable<CartItem> cartItemsList);
		Task<IEnumerable<CartItem>> GetCartItemsByUCartIdAsync(int cartId);
		Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId);
		Task<IEnumerable<CartItem>> GetCartItemsByProductSlugAsync(string productSlug);
		Task AddShoppingCartAsync(ShoppingCart shoppingCart);
		Task<int> SaveChangesAsync();
		void CartItemsRemoveRange(IEnumerable<CartItem> cartItems);
	}
}
