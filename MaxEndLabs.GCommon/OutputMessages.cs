using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.GCommon
{
	public class OutputMessages
	{
		public static class Order
		{
			public static string FailedToUpdateStatus = "Failed to update order status.";
			public static string FailedToCheckout = "Failed to checkout.";
			public static string FailedToUpdateToPaid = "Failed to update order status after payment. Please contact us!";
			public static string PaymentFailed = "Payment failed";
            public static string ServerError =
                "We encountered a technical issue with this product's data. Our team has been notified.";
        }

		public static class Product
		{
			public static string ProductCreated = "Product created!";
			public static string FailedToCreateProduct = "Failed to Create Product!";
			public static string ProductEdited= "Product edited!";
			public static string NoChangesWereMade = "No Changes were made!";
			public static string VariantUpdated = "Variants updated!";
			public static string ProductRemoved = "Product is removed!";
			public static string FailedToDeleteProduct = "The product remains published";
			public static string ProductRestored = "Product is restored!";
			public static string FailedToRestoreProduct = "Failed to restore product!";

            public static string ServerError =
                "We encountered a technical issue with this product's data. Our team has been notified.";

        }

		public static class ShoppingCart
		{
			public static string ProductAddedToCart = "Product added to cart!";
			public static string FailedToAddProductToCart = "Failed to add product to cart!";
			public static string ProductRemovedFromCart = "Product removed from cart!";
			public static string FailedToRemoveProductFromCart = "Failed to remove product from cart!";
            public static string ServerError =
                "We encountered a technical issue with this product's data. Our team has been notified.";
        }
	}
}
