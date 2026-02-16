using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
    public class CartItemRemoveViewModel
    {
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public int CartId {get; set; }
    }
}
