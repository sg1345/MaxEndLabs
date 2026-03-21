using System.ComponentModel.DataAnnotations;
using MaxEndLabs.Service.Models.ShoppingCart;

namespace MaxEndLabs.Service.Models.Order
{
    public class OrderCreateDto : ShoppingCartIndexDto
    {
        public string UserId { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Postcode { get; set; } = null!;
    } 
    
}
