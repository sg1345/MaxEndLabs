using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
	public class ProductIndexViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Slug { get; set; } = null!;
	}
}
