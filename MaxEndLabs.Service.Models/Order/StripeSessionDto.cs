using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
	public class StripeSessionDto
	{
		public Guid OrderId { get; set; }
		public string OrderNumber { get; set; } = null!;
		public List<StripeOrderItemDto> LineItems { get; set; } = [];
	}
}
