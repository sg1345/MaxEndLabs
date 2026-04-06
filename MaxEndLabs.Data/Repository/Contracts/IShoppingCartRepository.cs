using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IShoppingCartRepository
	{
		Task<Guid> GetShoppingCartIdAsync(Guid userId);
		Task AddToCartAsync(CartItem cartItem);
		void CartItemUpdate(CartItem cartItem);
		Task<CartItem?> GetCartItemIgnoreFilterAsync(Guid cartId, Guid productId, Guid productVariantId);
		Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId, Guid productVariantId);
		void CartItemUpdateRange(IEnumerable<CartItem> cartItemList);
		Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId);
		Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(Guid userId);
		Task<IEnumerable<CartItem>> GetCartItemsByProductSlugAsync(string productSlug);
        Task<IEnumerable<CartItem>> GetCartItemsByProductIdAndVariantIdAsync(Guid productId, Guid variantId);
        Task AddShoppingCartAsync(ShoppingCart shoppingCart);
		Task<int> SaveChangesAsync();
		void CartItemsRemoveRange(IEnumerable<CartItem> cartItems);
	}
}
