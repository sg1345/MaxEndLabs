using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Order
{
    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = null!;
        public string VariantName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal LineTotal { get; set; }

    }
}
