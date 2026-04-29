using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.NewsArticle;
using MaxEndLabs.Services.Core.Contracts;

namespace MaxEndLabs.Services.Core
{
    public class NewsArticleService :INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsArticleService(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<IEnumerable<NewsArticleSummaryDto>> GetNewsArticleSummariesAsync()
        {
            var newsArticles = await _newsArticleRepository.GetAllNewsArticlesAsync();

            if(newsArticles == null)
                throw new EntityNotFoundException();

            return newsArticles
                .OrderByDescending(na => na.PublishedAt)
                .Select(na => new NewsArticleSummaryDto
                {
                    Id = na.Id,
                    Summary = na.Summary,
                    TeaserTitle = na.TeaserTitle,
                    CoverImageUrl = na.CoverImageUrl
                });
        }

        public async Task<NewsArticleDetailsDto> GetNewsArticleDetailsAsync(Guid id)
        {
            var newsArticle = await _newsArticleRepository.GetNewsArticleByIdAsync(id);

            if(newsArticle == null)
                throw new EntityNotFoundException();

            return new NewsArticleDetailsDto()
            {
                Id = newsArticle.Id,
                ArticleImageUrl = newsArticle.ArticleImageUrl,
                Content = newsArticle.Content,
                ContentTitle = newsArticle.ContentTitle,
            };
        }

        public async Task AddNewsArticle(NewsArticleDto newNewsArticle)
        {
            var newsArticle = new NewsArticle()
            {
                IsPublished = true,
                ArticleImageUrl = newNewsArticle.ArticleImageUrl,
                Content = newNewsArticle.Content,
                ContentTitle = newNewsArticle.ContentTitle,
                PublishedAt = DateTime.UtcNow,
                CoverImageUrl = newNewsArticle.CoverImageUrl,
                Summary = newNewsArticle.Summary,
                TeaserTitle = newNewsArticle.TeaserTitle
            };

            await EnsureSaveChangesAsync();
        }

        private async Task EnsureSaveChangesAsync()
        {
            int changes = await _newsArticleRepository.SaveChangesAsync();

            var successAdd = changes > 0;

            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
