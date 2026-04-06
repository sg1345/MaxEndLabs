namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using static MaxEndLabs.GCommon.EntityValidation.ShoppingCart;

    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        [MaxLength(UserIdMaxLength)]
        public Guid UserId { get; set; }
		public virtual ApplicationUser User { get; set; } = null!;

		[Required]
        public DateTime CreatedAt { get; set; }
        
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
