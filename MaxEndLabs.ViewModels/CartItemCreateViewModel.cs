using System.ComponentModel.DataAnnotations;
using MaxEndLabs.GCommon;
using static MaxEndLabs.GCommon.EntityValidation.CartItem;
using static MaxEndLabs.GCommon.EntityValidation.Product;

namespace MaxEndLabs.ViewModels
{
	public class CartItemCreateViewModel
	{
		public int ProductId { get; set; }

		[Range(ProductVariantIdMinValue, int.MaxValue, ErrorMessage = "Please choose an Option")]
		public int ProductVariantId { get; set; }

		[Range(QuantityMinValue, QuantityMaxValue, ErrorMessage = "The quantity of the product should be between {1} and {2}")]
		public int Quantity { get; set; }

		//For the Details Action Redirect
		public string ProductSlug {get; set; } = null!;
		public string CategorySlug { get; set; } = null!;
	}

	}

