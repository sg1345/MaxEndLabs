using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Services.Core.Models.Configuration;
using MaxEndLabs.ViewModels.Order;
using MaxEndLabs.ViewModels.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using static MaxEndLabs.Web.Common.PaginationConstants;

namespace MaxEndLabs.Web.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IOrderService _orderService;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IOptions<GoogleReCaptchaSettings> _captchaSettings;
		private readonly IReCaptchaService _captchaService;
		private readonly IStripeService _stripeService;
		public OrderController(IOrderService orderService,
			IShoppingCartService shoppingCartService, IOptions<GoogleReCaptchaSettings> captchaSettings,
			IReCaptchaService captchaService, IStripeService stripeService)
		{
			_orderService = orderService;
			_shoppingCartService = shoppingCartService;
			_captchaSettings = captchaSettings;
			_captchaService = captchaService;
			_stripeService = stripeService;
		}
		
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Index(int page=1)
		{
			string userId = GetUserId()!;

			var orderDto = await _orderService.GetOrdersForUserAsync(userId, page, PageSizeOrderManager);

			var model = new OrderPaginationViewModel
			{
				CurrentPage = orderDto.CurrentPage,
				TotalPages = orderDto.TotalPages,
				HasNextPage = orderDto.HasNextPage,
				HasPreviousPage = orderDto.HasPreviousPage,
				Orders = orderDto.Orders.Select(o => new OrderViewModel
				{
					Id = o.Id,
					OrderNumber = o.OrderNumber,
					TotalAmount = o.TotalAmount,
					Status = o.Status,
					CreatedAt = o.CreatedAt
				}).ToList()
			};

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_OrderList", model);
			}

			return View(model);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> OrderManager(string searchTerm, string searchType, int page = 1)
		{
			var orderDto = await _orderService.GetOrderSearchAsync(searchTerm, searchType, page, PageSizeOrderManager);

			var model = new OrderPaginationViewModel
			{
				SearchTerm = searchTerm,
				SearchType = searchType,
				CurrentPage = orderDto.CurrentPage,
				TotalPages = orderDto.TotalPages,
				HasNextPage = orderDto.HasNextPage,
				HasPreviousPage = orderDto.HasPreviousPage,
				Orders = orderDto.Orders.Select(o => new OrderViewModel
				{
					Id = o.Id,
					OrderNumber = o.OrderNumber,
					TotalAmount = o.TotalAmount,
					Status = o.Status,
					CreatedAt = o.CreatedAt
				}).ToList()
			};

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_OrderList", model);
			}

			return View("Index", model);
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
                    })
                    .ToList()
            };

            ViewData["ReCaptchaSiteKey"] = _captchaSettings.Value.SiteKey;
			return View(model);
        }

        [HttpPost]
		[Authorize]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Checkout(CheckoutViewModel model)
		{
			string userId = GetUserId()!;
			var token = Request.Form["g-recaptcha-response"];
            var ok = await _captchaService.VerifyAsync(token);

            if (!ok)
            {
				ModelState.AddModelError(string.Empty, "Please confirm you’re not a robot.");

				await RefillCheckoutModel(model, userId);

				ViewData["ReCaptchaSiteKey"] = _captchaSettings.Value.SiteKey;
				return View(nameof(Checkout),model);
			}

            if (!ModelState.IsValid)
            {
	            await RefillCheckoutModel(model, userId);

				ViewData["ReCaptchaSiteKey"] = _captchaSettings.Value.SiteKey;
				return View(nameof(Checkout) ,model);
            }
            
            var createOrderDto = new AddressOrderDto()
            {
	            UserId = userId,
	            StreetAddress = model.StreetAddress,
	            City = model.City,
	            Postcode = model.Postcode,
            };

            var orderId = await _orderService.CreateOrderAsync(createOrderDto);
			if (orderId == 0)
				return RedirectToAction("Index","ShoppingCart");

			var cartId = await _shoppingCartService.GetOrCreateShoppingCart(userId);
			await _shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId);

			return RedirectToAction(nameof(StripeCheckout), new {orderId =  orderId});
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> StripeCheckout(int orderId)
		{
			var userId = GetUserId()!;
			var stripeSessionDto = await _orderService.GetOrderAsync(orderId);

			var baseUrl = Environment.GetEnvironmentVariable("APP__PUBLICBASEURL")
			              ?? $"{Request.Scheme}://{Request.Host}";
			var successBase = baseUrl + Url
				.Action(nameof(PaymentSuccess), "Order", new { stripeSessionDto.OrderId });
			var cancelUrl = baseUrl + Url.Action(nameof(PaymentCancel), "Order");
			var successSeparator = successBase.Contains('?') ? "&" : "?";
			var successUrl = successBase + successSeparator + "session_id={CHECKOUT_SESSION_ID}";

			var optionsFromService =
				_stripeService.CreateCheckoutSessionAsync(stripeSessionDto, successUrl, cancelUrl, userId);

			try
			{
				var session = await new SessionService().CreateAsync(optionsFromService);

				return Redirect(session.Url);
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> PaymentSuccess(int orderId,
			[FromQuery(Name = "session_id")] string sessionId)
		{

			if (string.IsNullOrWhiteSpace(sessionId))
			{
				Console.WriteLine($"[PaymentSuccess] Missing session_id. Path={Request.Path}, Query={Request.QueryString}");

				TempData["ErrorMessage"] = "Payment failed";
				return RedirectToAction(nameof(PaymentCancel));
			}
			
			//without webhook (offline)
			var service = new SessionService();
			var session = await service.GetAsync(sessionId);

			if (session.PaymentStatus == "paid")
			{
				string orderStatus = await _orderService.MarkOrderAsPaidAsync(orderId);
				return View("Done", model:orderStatus);
			}

			//With webhook (online)
			//var status = await _orderService.GetOrderStatusAsync(orderId);
			//if (status == "Paid")
			//{
			//	return View("Done");
			//}

			return View("PaymentProccessing", model:session.PaymentStatus);
		}


		[HttpGet]
		[Authorize]
		public IActionResult PaymentCancel()
		{
			return View();
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Details(int orderId)
        {
            var dto = await _orderService.GetOrderDetailsAsync(orderId);

            var model = new OrderDetailsViewModel
            {
				OwnerUserId = dto.OwnderUserId,
				OwnerFullName = dto.OwnerFullName,
				OwnerUsername = dto.OwnerUsername,
                CreatedAt = dto.CreatedAt,
				StatusBadgeClass = dto.StatusBadge,
                Status = dto.Status,
				StreetAddress = dto.StreetAddress,
                City = dto.City,
                Postcode = dto.Postcode,
                TotalAmount = dto.TotalAmount,
                OrderId = dto.OrderId,
                OrderNumber = dto.OrderNumber,
				Statuses = dto.Statuses,
                OrderItems = dto.LineItems.Select(li => new OrderItemViewModel
                    {
                        ProductName = li.ProductName,
                        VariantName = li.VariantName,
                        Quantity = li.Quantity,
                        Price = li.Price,
                        ImageUrl = li.ImageUrl,
						LineTotal = li.LineTotal
                    })
                    .ToList()
            };
            return PartialView("_OrderDetailsPartial",model);
        }

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
		{
			try
			{
				var orderStatus = await _orderService.GetOrderStatusAsync(orderId);

				if (orderStatus == newStatus)
                {
                    return await Details(orderId);
                }

				await _orderService.ChangeOrderStatus(newStatus, orderId);

                return await Details(orderId);

            }
			catch (Exception e)
			{
				return NotFound(e);
			}

		}

		private async Task RefillCheckoutModel(CheckoutViewModel model, string userId)
		{
			var checkOutDto = await _orderService.GetOrderCreateDtoAsync(userId);

			model.TotalPrice = checkOutDto.TotalPrice;
			model.CartId = checkOutDto.CartId;
			model.CartItems = checkOutDto.CartItems.Select(ci => new CartItemViewModel
			{
				ProductId = ci.ProductId,
				ProductName = ci.ProductName,
				ProductVariantId = ci.ProductVariantId,
				VariantName = ci.VariantName,
				UnitPrice = ci.UnitPrice,
				MainImageUrl = ci.MainImageUrl,
				Quantity = ci.Quantity
			}).ToList();
		}
	}
}
