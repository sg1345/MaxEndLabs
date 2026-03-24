using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Data.Repository
{
	public class ShoppingCartRepository : BaseRepository, IShoppingCartRepository
	{
		public ShoppingCartRepository(MaxEndLabsDbContext dbContext)
			: base(dbContext)
		{
		}

		public async Task<int> GetShoppingCartIdAsync(string userId)
		{
			return await DbContext.ShoppingCarts
				.AsNoTracking()
				.Where(sc => sc.UserId == userId)
				.Select(sc => sc.Id)
				.FirstOrDefaultAsync();
		}

		public async Task AddToCartAsync(CartItem cartItem)
		{
			await DbContext.CartItems.AddAsync(cartItem);
		}

		public void HardDeleteFromCartAsync(CartItem cartItem)
		{
			DbContext.CartItems.Remove(cartItem);
		}

		public void SoftDeleteFromCartAsync(CartItem cartItem)
		{
			cartItem.IsPublished = false;

			DbContext.CartItems.Update(cartItem);
		}

		public async Task<CartItem?> GetCartItemIgnoreFilterAsync(int cartId, int productId, int productVariantId)
		{
			return await DbContext.CartItems
				.IgnoreQueryFilters()
				.FirstOrDefaultAsync(ci => ci.CartId == cartId &&
										  ci.ProductId == productId &&
										  ci.ProductVariantId == productVariantId);

		}

		public async Task<CartItem?> GetCartItemAsync(int cartId, int productId, int productVariantId)
		{
			return await DbContext.CartItems
				.FirstOrDefaultAsync(ci => ci.CartId == cartId &&
				                           ci.ProductId == productId &&
				                           ci.ProductVariantId == productVariantId);

		}

		public void ClearCart(IEnumerable<CartItem> cartItemsList)
		{
			foreach (var cartItem in cartItemsList)
			{
				cartItem.IsPublished = false;
			}

			DbContext.CartItems.UpdateRange(cartItemsList);
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsByUCartIdAsync(int cartId)
		{
			return await DbContext.CartItems
				.Where(c => c.CartId == cartId)
				.ToListAsync();
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId)
		{
			return await DbContext.CartItems
				.AsNoTracking()
				.Include(ci => ci.Product)
				.Include(ci => ci.ProductVariant)
				.Where(ci => ci.ShoppingCart.UserId == userId)
				.ToArrayAsync();
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsByProductSlugAsync(string productSlug)
		{
			return await DbContext.CartItems
				.Where(ci => ci.Product.Slug == productSlug)
				.ToArrayAsync();

		}

		public async Task AddShoppingCartAsync(ShoppingCart shoppingCart)
		{
			await DbContext.ShoppingCarts.AddAsync(shoppingCart);
		}

		public void CartItemsRemoveRange(IEnumerable<CartItem> cartItems)
		{
			DbContext.CartItems.RemoveRange(cartItems);
		}
	}
}
