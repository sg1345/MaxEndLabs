using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Order
{
	public class OrderViewModel
	{
		public Guid Id { get; set; }
		public string OrderNumber { get; set; } = null!;
		public string Status { get; set; } = null!;
		public decimal TotalAmount { get; set; }
		public DateOnly CreatedAt { get; set; }
	}
}
