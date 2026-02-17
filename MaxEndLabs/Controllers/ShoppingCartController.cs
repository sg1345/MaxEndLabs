using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MaxEndLabs.Controllers
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
		public async Task<IActionResult> Index()
		{
				string? userId = GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToPage("/Account/Login", new { area = "Identity" });
				}

				var model = await _shoppingCartService.GetAllCartItemsAsync(userId);
				return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(CartItemCreateViewModel model)
		{
			string? userId = GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToPage("/Account/Login", new {area = "Identity"});
			}

			if (!ModelState.IsValid)
			{
				var productDetails = await _productService.GetProductDetailsAsync(model.CategorySlug, model.ProductSlug);
				return View("/Views/Products/Details.cshtml", productDetails);
			}

			var cartId = await _shoppingCartService.GetShoppingCartIdAsync(userId);
			if (cartId == 0)
			{
				cartId = await _shoppingCartService.CreateShoppingCartAsync(userId);
			}

			await _shoppingCartService.AddProductToShoppingCartAsync(model, cartId);

			return RedirectToAction("Details","Products", new { model.CategorySlug, model.ProductSlug });
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

				await _shoppingCartService.RemoveCartItemFromShoppingCartAsync(model);

				return RedirectToAction("Index");
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}

        }

		[HttpPost]
		public async Task<IActionResult> Checkout(int cartId)
		{
			try
			{
				string? userId = GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToPage("/Account/Login", new { area = "Identity" });
				}

				await _shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId);

				return View();
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
        }
	}
}
