using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data;
using MaxEndLabs.Data.Models;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Services.Core
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly MaxEndLabsDbContext _context;

		public ShoppingCartService(MaxEndLabsDbContext context)
		{
			_context = context;
		}

		public async Task<ShoppingCartIndexViewModel> GetAllCartItemsAsync(string userId)
		{
			var shoppingCartId = await _context.ShoppingCarts
                .AsNoTracking()
                .Where(sc => sc.UserId == userId)
                .Select(sc => sc.Id)
                .FirstOrDefaultAsync();

            var shoppingCartItemList = await _context.ShoppingCarts
				.AsNoTracking()
				.Include(sc => sc.CartItems)
				.ThenInclude(ci => ci.Product)
				.ThenInclude(p => p.ProductVariants)
				.Where(sc => sc.UserId == userId)
				.SelectMany(sc => sc.CartItems)
				.Select(ci => new ShoppingCartItemViewModel
				{
					ProductId = ci.ProductId,
					ProductName = ci.Product.Name,
					ProductVariantId = ci.ProductVariant!.Id,
					VariantName = ci.ProductVariant!.VariantName,
					UnitPrice = ci.ProductVariant.Price ?? ci.Product.Price,
					MainImageUrl = ci.Product.MainImageUrl,
					Quantity = ci.Quantity
				})
				.ToListAsync();

			if (shoppingCartItemList == null)
                throw new ArgumentException("Product Not Found");

            var model = new ShoppingCartIndexViewModel
            {
                TotalPrice = shoppingCartItemList.Sum(item => item.UnitPrice * item.Quantity),
				CartId = shoppingCartId,
                CartItems = shoppingCartItemList
            };

            return model;
		}

		public async Task AddProductToShoppingCartAsync(CartItemCreateViewModel model, int cartId)
		{
			var cartItem = await _context.CartItems
				.FirstOrDefaultAsync(ci => ci.CartId == cartId && 
				                           ci.ProductId == model.ProductId && 
				                           ci.ProductVariantId == model.ProductVariantId);

			if (cartItem == null)
			{
				var newCartItem = new CartItem
				{
					CartId = cartId,
					ProductId = model.ProductId,
					ProductVariantId = model.ProductVariantId,
					Quantity = model.Quantity
				};
				await _context.CartItems.AddAsync(newCartItem);
			}
			else
			{
				cartItem.Quantity += model.Quantity;
			}

			await _context.SaveChangesAsync();
		}

		public async Task RemoveCartItemFromShoppingCartAsync(CartItemRemoveViewModel model)
        {
			var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == model.CartId &&
                                           ci.ProductId == model.ProductId &&
                                           ci.ProductVariantId == model.ProductVariantId);

            if (cartItem == null)
                throw new ArgumentException("Cart Item Not Found");
            
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

		public async Task DeleteAllCartItemsFromShoppingCartAsync(int cartId)
		{
			var cartItemList = await _context.CartItems
                .Where(c => c.CartId == cartId)
                .ToListAsync();

			if(cartItemList == null || !cartItemList.Any())
				throw new ArgumentException("No Cart Items Found");

            _context.RemoveRange(cartItemList);
            await _context.SaveChangesAsync();
        }

		public async Task<int> GetShoppingCartIdAsync(string userId)
		{
			return await _context.ShoppingCarts
				.AsNoTracking()
				.Where(sc => sc.UserId == userId)
				.Select(sc => sc.Id)
				.FirstOrDefaultAsync();
		}

		public async Task<int> CreateShoppingCartAsync(string userId)
		{
			var newShoppingCart = new ShoppingCart
			{
				UserId = userId,
				CreatedAt = DateTime.UtcNow
			};

			await _context.AddAsync(newShoppingCart);
			await _context.SaveChangesAsync();

			return newShoppingCart.Id;
		}
	}
}
