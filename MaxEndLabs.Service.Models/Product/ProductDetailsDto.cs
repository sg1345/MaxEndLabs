using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Product
{
	public class ProductDetailsDto : ProductDto
	{
		public string? Description { get; set; }
		public IEnumerable<ProductVariantDto> ProductVariants { get; set; } = [];
	}
}
