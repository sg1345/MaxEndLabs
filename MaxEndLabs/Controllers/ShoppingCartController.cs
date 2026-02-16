using MaxEndLabs.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly IShoppingCartService _shoppingCartService;

		public ShoppingCartController(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}
		public async Task<IActionResult> Index()
		{
			return Ok("shopping cart works");
		}

		public async Task<IActionResult> AddToCart(string productSlug, string categorySlug)
		{
			return RedirectToAction("Details","Products", new { categorySlug, productSlug });
		}

		public async Task<IActionResult> RemoveFromCart(string productSlug)
		{
			return Ok("shopping cart item remove works");
		}

		public async Task<IActionResult> ClearCart()
		{
			return Ok("shopping cart clear works");
		}
	}
}
