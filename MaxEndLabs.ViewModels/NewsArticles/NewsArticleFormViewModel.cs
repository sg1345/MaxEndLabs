using System.ComponentModel.DataAnnotations;
using static MaxEndLabs.GCommon.EntityValidation.NewsArticle;

namespace MaxEndLabs.ViewModels.NewsArticles
{
    public class NewsArticleFormViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter a content title")]
        [StringLength(ContentTitleMaxLength, MinimumLength = ContentTitleMinLength, 
            ErrorMessage = "Content title must be between {2} and {1} characters")]
        public string ContentTitle { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a content")]
        public string Content { get; set; } = null!;

        public string? ArticleImageUrl { get; set; }

        [Required(ErrorMessage = "Please enter a teaser title")]
        [StringLength(TeaserTittleMaxLength, MinimumLength = TeaserTittleMinLength,
            ErrorMessage = "Teaser title must be between {2} and {1} characters")]
        public string TeaserTitle { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a summary")]
        [StringLength(SummaryMaxLength, MinimumLength = SummaryMinLength,
            ErrorMessage = "Content title must be between {2} and {1} characters")]
        public string Summary { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an URL")]
        public string CoverImageUrl { get; set; } = null!;
    }
}
