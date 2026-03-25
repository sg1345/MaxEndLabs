using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.Order
{
    public class OrderDetailsDto 
    {
        public string OwnerFullName { get; set; } = null!;
        public string OwnerUsername { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public string StatusBadge { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public List<OrderItemDetailsDto> LineItems { get; set; } = [];
    }
}
