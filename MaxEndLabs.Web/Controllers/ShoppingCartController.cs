using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.Product;
using MaxEndLabs.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MaxEndLabs.GCommon.OutputMessages.ShoppingCart;
using static MaxEndLabs.GCommon.ApplicationConstants;

namespace MaxEndLabs.Web.Controllers
{
	public class ShoppingCartController : BaseController
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IProductService _productService;

		public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
		{

			_shoppingCartService = shoppingCartService;
			_productService = productService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			string? userId = GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}

			try
			{
				var dto = await _shoppingCartService
					.GetAllCartItemsAsync(userId);

				var model = new ShoppingCartIndexViewModel()
				{
					CartId = dto.CartId,
					TotalPrice = dto.TotalPrice,
					CartItems = dto.CartItems.Select(ci => new CartItemViewModel
					{
						ProductId = ci.ProductId,
						ProductName = ci.ProductName,
						ProductVariantId = ci.ProductVariantId,
						VariantName = ci.VariantName,
						UnitPrice = ci.UnitPrice,
						MainImageUrl = ci.MainImageUrl,
						Quantity = ci.Quantity
					})
						.ToList()
				};

				return View(model);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> AddToCart(CartItemCreateViewModel model)
		{
			string? userId = GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });

			}

			try
			{
				if (!ModelState.IsValid)
				{
					var productDetailsDto = await _productService.GetProductDetailsAsync(model.ProductSlug, isFiltered: true);

					var productDetails = new ProductDetailsViewModel()
					{
						Id = productDetailsDto.Id,
						Name = productDetailsDto.Name,
						ProductSlug = productDetailsDto.Slug,
						CategorySlug = productDetailsDto.CategorySlug,
						Description = productDetailsDto.Description,
						Price = productDetailsDto.Price,
						MainImageUrl = productDetailsDto.MainImageUrl,
						IsPublished = productDetailsDto.IsPublished,
						ProductVariants = productDetailsDto.ProductVariants
							.Select(av => new VariantDisplayViewModel()
							{
								Id = av.Id,
								VariantName = av.VariantName,
								Price = av.Price
							}).ToList(),
					};
					TempData["ErrorMessage"] = "Failed adding to cart";
					return View("/Views/Products/Details.cshtml", productDetails);
				}

				var cartId = await _shoppingCartService.GetOrCreateShoppingCart(userId);

				var dto = new CartItemCreateDto()
				{
					CartId = cartId,
					ProductId = model.ProductId,
					ProductVariantId = model.ProductVariantId,
					Quantity = model.Quantity
				};

				await _shoppingCartService.AddProductToShoppingCartAsync(dto);

				TempData["SuccessMessage"] = ProductAddedToCart;
				return RedirectToAction("Details", "Products", new
				{
					model.CategorySlug,
					model.ProductSlug
				});
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[SuccessTempDataKey] = FailedToAddProductToCart;
				return RedirectToAction("Details", "Products", new
				{
					model.CategorySlug,
					model.ProductSlug
				});
			}


		}

		[HttpPost]
		public async Task<IActionResult> RemoveFromCart(CartItemRemoveViewModel model)
		{
			try
			{
				string? userId = GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToPage("/Account/Login", new { area = "Identity" });
				}

				var dto = new CartItemDeleteDto()
				{
					CartId = model.CartId,
					ProductId = model.ProductId,
					ProductVariantId = model.ProductVariantId
				};

				await _shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto);

				TempData[SuccessTempDataKey] = ProductRemovedFromCart;
				return RedirectToAction("Index");
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch(EntityPersistFailureException e)
			{
				TempData[ErrorTempDataKey] = FailedToRemoveProductFromCart;
				return RedirectToAction("Index");
			}
		}
	}
}
