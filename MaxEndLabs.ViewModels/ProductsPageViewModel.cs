using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class ProductsPageViewModel
	{
		public string Title { get; set; } = null!;
		public IEnumerable<ProductListViewModel> Products { get; set; } = [];

	}
}
