using MaxEndLabs.Service.Models.Order;
using Stripe.Checkout;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IStripeService
	{
		SessionCreateOptions CreateCheckoutSessionAsync(StripeSessionDto dto, string successUrl, string cancelUrl,string userId);
	}
}
