using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IShoppingCartRepository
	{
		Task<int> GetShoppingCartIdAsync(string userId);
		Task AddToCartAsync(CartItem cartItem);
		void CartItemUpdate(CartItem cartItem);
		Task<CartItem?> GetCartItemIgnoreFilterAsync(int cartId, int productId, int productVariantId);
		Task<CartItem?> GetCartItemAsync(int cartId, int productId, int productVariantId);
		void CartItemUpdateRange(IEnumerable<CartItem> cartItemList);
		Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
		Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId);
		Task<IEnumerable<CartItem>> GetCartItemsByProductSlugAsync(string productSlug);
        Task<IEnumerable<CartItem>> GetCartItemsByProductIdAndVariantIdAsync(int productId, int variantId);
        Task AddShoppingCartAsync(ShoppingCart shoppingCart);
		Task<int> SaveChangesAsync();
		void CartItemsRemoveRange(IEnumerable<CartItem> cartItems);
	}
}
