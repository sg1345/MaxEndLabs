using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Product
{
    public class ManageVariantsViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSlug { get; set; }
        public string? CategorySlug { get; set; }

        public List<VariantEditViewModel> Variants { get; set; } = [];
    }
}
