using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Services.Core.Models.Configuration;
using MaxEndLabs.ViewModels.Order;
using MaxEndLabs.ViewModels.ShoppingCart;
using MaxEndLabs.Web.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace MaxEndLabs.Web.Controllers
{
	public class OrderController : BaseController
	{
		private readonly IOrderService _orderService;
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IOptions<GoogleReCaptchaSettings> _captchaSettings;
		private readonly IReCaptchaService _captchaService;
		public OrderController(IOrderService orderService,
			IShoppingCartService shoppingCartService,
			IOptions<GoogleReCaptchaSettings> captchaSettings,
			IReCaptchaService captchaService)
		{
			_orderService = orderService;
			_shoppingCartService = shoppingCartService;
			_captchaSettings = captchaSettings;
			_captchaService = captchaService;
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
                    })
                    .ToList()
            };

            ViewData["ReCaptchaSiteKey"] = _captchaSettings.Value.SiteKey;
			return View(model);
        }


        [HttpPost]
		[Authorize]
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
				return View(model);
			}

            if (!ModelState.IsValid)
            {
	            await RefillCheckoutModel(model, userId);

				ViewData["ReCaptchaSiteKey"] = _captchaSettings.Value.SiteKey;
				return View(model);
            }
            

            var createOrderDto = new AddressOrderDto()
            {
	            UserId = userId,
	            StreetAddress = model.StreetAddress,
	            City = model.City,
	            Postcode = model.Postcode,

            };
			//Create pending Order
            var stripeSessionDto = await _orderService.CreateOrderAsync(createOrderDto);
			await _shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(stripeSessionDto.CartId);

			if (stripeSessionDto is null || !stripeSessionDto.LineItems.Any())
				return RedirectToAction(nameof(Checkout));

			var lineItems = stripeSessionDto.LineItems.Select(li =>
				{
					return new SessionLineItemOptions
					{
						Quantity = li.Quantity,
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = "eur",
							UnitAmount = (long)li.Price,
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = li.ProductName,
								Images = string.IsNullOrWhiteSpace(li.ImageUrl)
									? null
									: new List<string> { li.ImageUrl }
							}
						}
					};
				})
				.ToList();

			var baseUrl = Environment.GetEnvironmentVariable("APP__PUBLICBASEURL")
						  ?? $"{Request.Scheme}://{Request.Host}";
			var successBase = baseUrl + Url
				.Action(nameof(PaymentSuccess), "Order", new { stripeSessionDto.OrderId });
			var cancelUrl = baseUrl + Url.Action(nameof(PaymentCancel), "Order");
			var successSeparator = successBase.Contains('?') ? "&" : "?";
			var successUrl = successBase + successSeparator + "session_id={CHECKOUT_SESSION_ID}";


			var options = new SessionCreateOptions
			{
				Mode = "payment",
				PaymentMethodTypes = new List<String> { "card" },
				LineItems = lineItems,

				SuccessUrl = successUrl,
				CancelUrl = cancelUrl,

				Metadata = new Dictionary<string, string>
				{
					["orderId"] = stripeSessionDto.OrderId.ToString(),
					["userId"] = userId
				}
			};


			try
			{
				var session = await new SessionService().CreateAsync(options);

				return Redirect(session.Url);
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpGet]
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

			return View("PaymentProccessing");
		}


		[HttpGet]
		public IActionResult PaymentCancel()
		{
			return View();
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
