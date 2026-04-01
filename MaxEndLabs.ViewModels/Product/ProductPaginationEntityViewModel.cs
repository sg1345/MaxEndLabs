using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.Product
{
    public class ProductPaginationEntityViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Slug { get; set; } = null!;
        public string? MainImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategorySlug { get; set; } = null!;
    }
}
