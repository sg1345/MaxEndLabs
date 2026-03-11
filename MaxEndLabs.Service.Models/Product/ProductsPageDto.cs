using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Product
{
	public class ProductsPageDto
	{
		public string Title { get; set; } = null!;
		public IEnumerable<ProductDto> Products { get; set; } = [];
	}
}
