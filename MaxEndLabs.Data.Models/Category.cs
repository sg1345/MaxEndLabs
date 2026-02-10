namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static MaxEndLabs.Common.EntityValidation.Category;
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; } = null!;

        [ForeignKey(nameof(ParentCategory))]
        public int? ParentCategoryId { get; set; }

        [InverseProperty(nameof(Subcategories))]
        public virtual Category? ParentCategory { get; set; }

        public string? Description { get; set; }

        [InverseProperty(nameof(ParentCategory))]
        public virtual ICollection<Category> Subcategories { get; set; } = new HashSet<Category>();

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
