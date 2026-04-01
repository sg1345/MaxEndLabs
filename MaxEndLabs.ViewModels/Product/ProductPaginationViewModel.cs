using MaxEndLabs.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Product
{
    public class ProductPaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<ProductPaginationEntityViewModel> Products { get; set; } = [];

        public string? SearchTerm { get; set; }
    }
}
