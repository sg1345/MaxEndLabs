namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static MaxEndLabs.Common.EntityValidation.Product;
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public bool IsDigital { get; set; }

        [Required]
        [Column(TypeName = PriceColumnType)]
        public decimal Price { get; set; }

        //nvarchar(max) in on purpose to allow for long URLs 
        public string? MainImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public bool IsPublished { get; set; }
        [Required]
        public bool StockTracked { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new HashSet<ProductVariant>();
        
    }
}
