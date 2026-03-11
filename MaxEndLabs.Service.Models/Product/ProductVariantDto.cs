using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Product
{
	public class ProductVariantDto
	{
		public int Id { get; set; }
		public string VariantName { get; set; } = null!;
		public decimal Price { get; set; }
	}
}
