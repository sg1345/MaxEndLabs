using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaxEndLabs.GCommon.EntityValidation.Product;

namespace MaxEndLabs.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Please enter a product name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} characters")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please pick a Category")]
        [Range(CategoryIdMinValue,CategoryIdMaxValue)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a product price")]
        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = "Price must be between 0.01 and 9,999,999.99")]
        public decimal Price { get; set; }

        public string? MainImageUrl { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = [];
    }
}
