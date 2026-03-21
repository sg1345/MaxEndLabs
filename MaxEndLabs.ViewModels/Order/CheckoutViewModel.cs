using System.ComponentModel.DataAnnotations;
using MaxEndLabs.ViewModels.ShoppingCart;
using static MaxEndLabs.GCommon.EntityValidation.Order;

namespace MaxEndLabs.ViewModels.Order
{
    public class CheckoutViewModel : ShoppingCartIndexViewModel
    {
        [Required]
        [StringLength(StreetAddressMaxLength, MinimumLength = StreetAddressMinLength, 
            ErrorMessage = "Address must be between {2} and {1} characters")]
        public string StreetAddress { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength,
            ErrorMessage = "City must be between {2} and {1} characters")]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(PostcodeMaxLength, MinimumLength = PostcodeMinLength,
            ErrorMessage = "Postcode must be between {2} and {1} characters")]
        public string Postcode { get; set; } = null!;
    }
}
