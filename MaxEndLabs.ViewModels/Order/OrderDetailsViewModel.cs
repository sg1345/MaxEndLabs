using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
	    public string OwnerUserId { get; set; } = null!;
        public string OwnerFullName { get; set; } = null!;
        public string OwnerUsername { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string StatusBadgeClass { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public List<OrderItemViewModel> OrderItems { get; set; } = [];

        public List<String> Statuses = [];
    }
}
