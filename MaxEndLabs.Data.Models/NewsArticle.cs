using System.ComponentModel.DataAnnotations;
using static MaxEndLabs.GCommon.EntityValidation.NewsArticle;

namespace MaxEndLabs.Data.Models
{
    public class NewsArticle
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TeaserTittleMaxLength)]
        public string TeaserTitle { get; set; } = null!;

        [Required]
        [MaxLength(ContentTitleMaxLength)]
        public string ContentTitle { get; set; } = null!;

        [Required]
        [MaxLength(SummaryMaxLength)]
        public string Summary { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public string CoverImageUrl { get; set; } = null!;

        public string? ArticleImageUrl { get; set; }

        public DateTime PublishedAt { get; set; }

        public bool IsPublished { get; set; }
    }
}
