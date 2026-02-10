using Microsoft.AspNetCore.Identity;

namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
     using static MaxEndLabs.Common.EntityValidation.ShoppingCart;

    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        [MaxLength(UserIdMaxLength)]
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
