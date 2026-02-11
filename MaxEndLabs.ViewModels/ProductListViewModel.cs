using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class ProductListViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Slug { get; set; } = null!;
		public int CategoryId { get; set; }
		public decimal Price { get; set; }
		public string? MainImageUrl { get; set; }
	}
}
