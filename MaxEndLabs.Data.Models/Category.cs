namespace MaxEndLabs.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static MaxEndLabs.GCommon.EntityValidation.Category;
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SlugMaxLength)]
        public string Slug { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
