namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static MaxEndLabs.GCommon.EntityValidation.ProductVariant;
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Required]
        [MaxLength(VariantNameMaxLength)]
        public string VariantName { get; set; } = null!;


        //[Column(TypeName = PriceColumnType)]
        public decimal? Price { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
	}
}
