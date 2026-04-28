using MaxEndLabs.Service.Models.NewsArticle;

namespace MaxEndLabs.Services.Core.Contracts
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticleSummaryDto>> GetNewsArticleSummariesAsync();
        Task<NewsArticleDetailsDto> GetNewsArticleDetailsAsync(Guid id);

        Task AddNewsArticle(NewsArticleDto newNewsArticle);
    }
}
