namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ShoppingCart))]
        public int CartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(ProductVariant))]
        public int ProductVariantId { get; set; }

        public virtual ProductVariant ProductVariant { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime AddedAt { get; set; }             
    }
}
