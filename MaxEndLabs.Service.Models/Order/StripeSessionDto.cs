using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
	public class StripeSessionDto
	{
		public int CartId { get; set; }
		public int OrderId { get; set; }
		public string OrderNumber { get; set; } = null!;
		public List<StripeLineItemDto> LineItems { get; set; } = [];
	}
}
