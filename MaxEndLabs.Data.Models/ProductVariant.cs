namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static MaxEndLabs.Common.EntityValidation.ProductVariant;
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Required]
        [MaxLength(VariantNameMaxLength)]
        public string VariantName { get; set; } = null!;


        [Column(TypeName = PriceColumnType)]
        public decimal? Price { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}
