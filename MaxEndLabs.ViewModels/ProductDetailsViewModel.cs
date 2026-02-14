using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class ProductDetailsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string ProductSlug { get; set; } = null!;
		public string CategorySlug { get; set; } = null!;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public string? MainImageUrl { get; set; }

		public IEnumerable<VariantDisplayViewModel> ProductVariants { get; set; } = [];

	}
}
