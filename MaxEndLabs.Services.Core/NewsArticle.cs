using MaxEndLabs.Service.Models.NewsArticle;
using MaxEndLabs.Services.Core.Contracts;

namespace MaxEndLabs.Services.Core
{
    public class NewsArticle :INewsArticleService
    {
        public Task<IEnumerable<NewsArticleSummaryDto>> GetNewsArticleSummariesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NewsArticleDetailsDto> GetNewsArticleDetailsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task AddNewsArticle(NewsArticleDto newNewsArticle)
        {
            throw new NotImplementedException();
        }
    }
}
