using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Services.Core.Contracts;
using Stripe.Checkout;

namespace MaxEndLabs.Services.Core
{
	public class StripeService : IStripeService
	{
		public SessionCreateOptions CreateCheckoutSessionAsync(
			StripeSessionDto dto, string successUrl, string cancelUrl, Guid userId)
		{
			var lineItems = dto.LineItems
				.Select(li => new SessionLineItemOptions
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
				})
				.ToList();


			return new SessionCreateOptions
			{
				Mode = "payment",
				PaymentMethodTypes = new List<String> { "card" },
				LineItems = lineItems,

				SuccessUrl = successUrl,
				CancelUrl = cancelUrl,

				Metadata = new Dictionary<string, string>
				{
					["orderId"] = dto.OrderId.ToString(),
					["userId"] = userId.ToString()
				}
			};
		}
	}
}
