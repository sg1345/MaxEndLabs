using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Task AddToCartAsync(int userId, int productVariantId, int quantity)
		{
			throw new NotImplementedException();
		}

		public Task RemoveFromCartAsync(int userId, int productVariantId)
		{
			throw new NotImplementedException();
		}

		public Task UpdateCartItemAsync(int userId, int productVariantId, int quantity)
		{
			throw new NotImplementedException();
		}

		public Task ClearCartAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string productSlug)
		{
			return await DbContext.CartItems
				.Where(ci => ci.Product.Slug == productSlug)
				.ToArrayAsync();

		}

		public void CartItemsRemoveRange(IEnumerable<CartItem> cartItems)
		{
			DbContext.CartItems.RemoveRange(cartItems);
		}
	}
}
