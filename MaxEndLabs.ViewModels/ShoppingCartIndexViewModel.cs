using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels
{
    public class ShoppingCartIndexViewModel
    {
        public decimal TotalPrice { get; set; }
        public  int CartId { get; set; }
        public List<ShoppingCartItemViewModel> CartItems { get; set; } = new List<ShoppingCartItemViewModel>();
    }
}
