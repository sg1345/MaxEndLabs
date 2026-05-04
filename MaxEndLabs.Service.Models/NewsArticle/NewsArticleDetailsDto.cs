namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticleDetailsDto 
    {
        public Guid Id { get; set; }
        public string ContentTitle { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ArticleImageUrl { get; set; }
    }
}
