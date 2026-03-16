using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Data.Models.Enum
{
	public enum OrderStatus
	{
		Pending = 1,
		Paid = 2,
		Shipped = 3,
		Completed = 4,
		Cancelled = 5,
		Refunded = 6
	}
}
