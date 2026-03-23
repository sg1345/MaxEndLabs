using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace MaxEndLabs.Web.Controllers
{
	[ApiController]
	public class StripeController : ControllerBase
	{
		private readonly StripeSettings _stripeSettings;

		private readonly IOrderService _orderService;


		public StripeController(IOptions<StripeSettings> stripeOptions, IOrderService orderService)
		{
			this._stripeSettings = stripeOptions.Value;
			this._orderService = orderService;
		}


		[HttpPost("stripe/webhook")]
		public async Task<IActionResult> Webhook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			var stripeSignature = Request.Headers["Stripe-Signature"];

			Event stripeEvent;
			try
			{
				stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _stripeSettings.WebhookSecret);

			}
			catch
			{
				return BadRequest();
			}

			if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
			{
				var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

				if (session?.Metadata != null &&
				    session.Metadata.TryGetValue("orderId", out var orderIdStr) &&
				    int.TryParse(orderIdStr, out var orderId))
				{
					await _orderService.MarkOrderAsPaidAsync(orderId);
				}
			}

			return Ok();
		}
	}
}