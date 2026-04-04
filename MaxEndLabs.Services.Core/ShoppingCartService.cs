using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;


namespace MaxEndLabs.Services.Core
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;

		public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
		}

		public async Task<ShoppingCartIndexDto> GetAllCartItemsAsync(string userId)
		{
			int shoppingCartId = await _shoppingCartRepository.GetShoppingCartIdAsync(userId);

			var cartItemList = await _shoppingCartRepository.GetCartItemsByUserIdAsync(userId);

			if(cartItemList == null)
				throw new EntityNotFoundException();

			var cartItemListDto = cartItemList
				.Select(ci => new CartItemDto
				{
					ProductId = ci.ProductId,
					ProductName = ci.Product.Name,
					ProductVariantId = ci.ProductVariant!.Id,
					VariantName = ci.ProductVariant!.VariantName,
					UnitPrice = ci.ProductVariant.Price ?? ci.Product.Price,
					MainImageUrl = ci.Product.MainImageUrl,
					Quantity = ci.Quantity
				})
				.ToList();

            var shoppingCartIndexDto = new ShoppingCartIndexDto
            {
                TotalPrice = cartItemListDto.Sum(item => item.UnitPrice * item.Quantity),
				CartId = shoppingCartId,
                CartItems = cartItemListDto
            };

            return shoppingCartIndexDto;
		}

		public async Task AddProductToShoppingCartAsync(CartItemCreateDto dto)
		{
			var cartItem = await _shoppingCartRepository
				.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.ProductVariantId);


			if (cartItem == null)
			{
				var newCartItem = new CartItem
				{
					CartId = dto.CartId,
					ProductId = dto.ProductId,
					ProductVariantId = dto.ProductVariantId,
					Quantity = dto.Quantity,
					AddedAt = DateTime.UtcNow,
					IsPublished = true	
				};
				await _shoppingCartRepository.AddToCartAsync(newCartItem);
			}
			else if (!cartItem.IsPublished)
			{
				cartItem.IsPublished = true;
				cartItem.AddedAt = DateTime.UtcNow;
				cartItem.Quantity = dto.Quantity;
			}
			else
			{
				cartItem.Quantity += dto.Quantity;
			}

			await EnsureSaveChangesAsync();
		}

		public async Task RemoveCartItemFromShoppingCartAsync(CartItemDeleteDto dto)
		{
			var cartItem = await _shoppingCartRepository
					.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId);

			if (cartItem == null)
                throw new EntityNotFoundException();

            if (cartItem.IsPublished)
            {
                cartItem.IsPublished = false;

                _shoppingCartRepository.CartItemUpdate(cartItem);
            }
            
            await EnsureSaveChangesAsync();
		}

		public async Task DeleteAllCartItemsFromShoppingCartAsync(int cartId)
		{
			var cartItemList = await _shoppingCartRepository
				.GetCartItemsByCartIdAsync(cartId);

			if(cartItemList == null)
				throw new EntityNotFoundException();

            bool changesMade = false;
            foreach (var cartItem in cartItemList)
            {
                if (cartItem.IsPublished)
                {
                    cartItem.IsPublished = false;
					changesMade = true;
                }
                    
            }

            if (changesMade)
                _shoppingCartRepository.CartItemUpdateRange(cartItemList);

            await EnsureSaveChangesAsync();
		}

		public async Task<int> GetOrCreateShoppingCartAsync(string userId)
		{
			var carId = await _shoppingCartRepository.GetShoppingCartIdAsync(userId);

			if (carId == 0)
			{
				var newShoppingCart = new ShoppingCart
				{
					UserId = userId,
					CreatedAt = DateTime.UtcNow
				};

				await _shoppingCartRepository.AddShoppingCartAsync(newShoppingCart);

				await EnsureSaveChangesAsync();

				carId = newShoppingCart.Id;
			}

			return carId;
		}

		private async Task EnsureSaveChangesAsync()
		{
			int changes = await _shoppingCartRepository.SaveChangesAsync();

			var successAdd = changes > 0;

			if (!successAdd)
			{
				throw new EntityPersistFailureException();
			}
		}
	}
}
