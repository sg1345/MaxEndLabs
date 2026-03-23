using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
	public class StripeLineItemDto
	{
		public string ProductName { get; set; } = null!;
		public string VariantName { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string? ImageUrl { get; set; }
	}
	
}
