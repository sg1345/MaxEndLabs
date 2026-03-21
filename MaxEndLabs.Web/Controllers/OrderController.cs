using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.Order;
using MaxEndLabs.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IOrderService _orderService;
		private readonly IShoppingCartService _shoppingCartService;

		public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService)
		{
			_orderService = orderService;
			_shoppingCartService = shoppingCartService;
		}

		public IActionResult Index()
		{
			return Ok();
		}

        [HttpGet]
		[Authorize]
        public async Task<IActionResult> Checkout()
        {
			string userId = GetUserId()!;
            var checkOutDto = await _orderService.GetOrderCreateDtoAsync(userId);

            var model = new CheckoutViewModel
            {
                TotalPrice = checkOutDto.TotalPrice,
                CartId = checkOutDto.CartId,
                CartItems = checkOutDto.CartItems.Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.ProductName,
                    ProductVariantId = ci.ProductVariantId,
                    VariantName = ci.VariantName,
                    UnitPrice = ci.UnitPrice,
                    MainImageUrl = ci.MainImageUrl,
                    Quantity = ci.Quantity
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
		[Authorize]
		public async Task<IActionResult> Checkout(CheckoutViewModel model)
		{

            if (!ModelState.IsValid)
            {
                return View(model);
            }

			try
			{
                string userId = GetUserId()!;

                var createOrderDto = new OrderCreateDto
                {
                    UserId = userId,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    Postcode = model.Postcode,
                    TotalPrice = model.TotalPrice,
                    CartId = model.CartId,
                    CartItems = model.CartItems.Select(ci => new CartItemDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.ProductName,
                        ProductVariantId = ci.ProductVariantId,
                        VariantName = ci.VariantName,
                        UnitPrice = ci.UnitPrice,
                        MainImageUrl = ci.MainImageUrl,
                        Quantity = ci.Quantity
                    }).ToList()
                };

                int cartId = await _orderService.CreateOrderAsync(createOrderDto);

                await _shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId);

				return View("Done");
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}
	}
}
