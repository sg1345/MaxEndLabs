using System.ComponentModel.DataAnnotations;
using static MaxEndLabs.GCommon.EntityValidation.ProductVariant;

namespace MaxEndLabs.ViewModels.Product
{
    public class VariantEditViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please enter a Option Name")]
        [StringLength(VariantNameMaxLength, MinimumLength = VariantNameMinLength, ErrorMessage = "Option should be between {2} and {1} characters")]
        public string Name { get; set; } = null!;

        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = "Price must be between 0.01 and 9,999,999.99")]
        public decimal? Price { get; set; }
    }
}
