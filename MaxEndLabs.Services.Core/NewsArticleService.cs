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

        public async Task<NewsArticlePaginationDto> GetNewsArticleSummariesAsync(string searchTerm, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var newsArticles = await _newsArticleRepository
                .GetNewsArticlesSearchAsync(searchTerm, skip,pageSize);
            var count = await _newsArticleRepository.GetCountAsync(searchTerm);

            if(newsArticles == null)
                throw new EntityNotFoundException();

            bool hasPreviousPage = page > 1;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new NewsArticlePaginationDto
                {
                CurrentPage = page,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = page < totalPages,
                Articles = newsArticles.Select(na => new NewsArticleSummaryDto
                {
                    Id = na.Id,
                    TeaserTitle = na.TeaserTitle,
                    CoverImageUrl = na.CoverImageUrl,
                    Summary = na.Summary
                })
            };
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

            await _newsArticleRepository.AddNewsArticleAsync(newsArticle);

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
