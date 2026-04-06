using System.ComponentModel.DataAnnotations;
using static MaxEndLabs.GCommon.EntityValidation.Product;
using MaxEndLabs.ViewModels.Category;

namespace MaxEndLabs.ViewModels.Product
{
    public class ProductFormViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter a product name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} characters")]
        public string Name { get; set; } = null!;


        [StringLength(DescriptionMaxLength, ErrorMessage = "Description cannot be more than {1} characters")]
        public string? Description { get; set; }

        //[Required(ErrorMessage = "Please pick a Category")]
        //[Range(CategoryIdMinValue, CategoryIdMaxValue)]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a product price")]
        [Range(PriceMinValue, PriceMaxValue, ErrorMessage = "Price must be between 0.01 and 9,999,999.99")]
        public decimal Price { get; set; }

        public string? MainImageUrl { get; set; }

        public IEnumerable<CategorySelectViewModel> Categories { get; set; } = [];
    }
}
