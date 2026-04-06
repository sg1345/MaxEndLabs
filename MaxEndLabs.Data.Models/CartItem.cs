namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(ShoppingCart))]
        public Guid CartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(ProductVariant))]
        public Guid ProductVariantId { get; set; }

        public virtual ProductVariant ProductVariant { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime AddedAt { get; set; } 
        
        [Required]
        public bool IsPublished { get; set; } 
    }
}
