using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
    public class OrderItemDetailsDto : StripeOrderItemDto
    {
        public decimal LineTotal { get; set; }
    }
}
