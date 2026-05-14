using MaxEndLabs.Service.Models.NewsArticle;

namespace MaxEndLabs.Services.Core.Contracts
{
    public interface INewsArticleService
    {
        Task<NewsArticlePaginationDto> GetNewsArticleSummariesAsync(string searchTerm, int page, int pageSize);
        Task<NewsArticleDetailsDto> GetNewsArticleDetailsAsync(Guid id);

        Task AddNewsArticle(NewsArticleDto newNewsArticle);
    }
}
