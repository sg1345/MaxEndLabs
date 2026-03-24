using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
	public class OrderDto
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; } = null!;
		public string Status { get; set; } = null!;
		public decimal TotalAmount { get; set; }
		public DateOnly CreatedAt { get; set; }
	}
}
