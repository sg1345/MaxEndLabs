using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class VariantDisplayViewModel
	{
		public int Id { get; set; }
		public string VariantName { get; set; } = null!;
		public decimal Price { get; set; }
	}
}
