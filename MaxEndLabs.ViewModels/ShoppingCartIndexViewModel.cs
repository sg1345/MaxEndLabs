using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class ShoppingCartIndexViewModel
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public int VariantId { get; set; }
		public string VariantName { get; set; } = null!;
		public decimal UnitPrice { get; set; }
		public string MainImageUrl { get; set; } = null!;
		public int Quantity { get; set; }
	}
}
