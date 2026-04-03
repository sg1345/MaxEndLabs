using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
	public class OrderPaginationDto
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public bool HasPreviousPage { get; set; }
		public bool HasNextPage { get; set; }
		public IEnumerable<OrderDto> Orders { get; set; } = [];
	}
}
