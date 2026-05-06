using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Data.Repository
{
    public class NewsArticleRepository : BaseRepository, INewsArticleRepository
    {
        public NewsArticleRepository(MaxEndLabsDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<NewsArticle>?> GetNewsArticlesSearchAsync(string? searchTerm, int skip, int take)
        {
            IQueryable<NewsArticle> query = DbContext.NewsArticles
                .AsNoTracking()
                .OrderBy(na => na.TeaserTitle)
                .ThenBy(na => na.TeaserTitle)
                .ThenByDescending(na => na.PublishedAt);

            if (searchTerm != null)
            {
                return await query
                    .Where(na => na.ContentTitle.Contains(searchTerm) || na.TeaserTitle.Contains(searchTerm))
                    .Skip(skip)
                    .Take(take)
                    .ToArrayAsync();
            }
            else if (string.IsNullOrEmpty(searchTerm))
            {
                return await query
                    .Skip(skip)
                    .Take(take)
                    .ToArrayAsync();
            }

            return null;
        }

        public async Task<int> GetCountAsync(string? searchTerm)
        {
            IQueryable<NewsArticle> query = DbContext.NewsArticles
                .AsNoTracking();

            if (searchTerm != null)
            {
                return await query
                    .CountAsync(na => na.ContentTitle.Contains(searchTerm) || na.TeaserTitle.Contains(searchTerm));
            }
            else if (string.IsNullOrEmpty(searchTerm))
            {
                return await query
                    .CountAsync();
            }

            return 0;
        }

        public async Task<NewsArticle?> GetNewsArticleByIdAsync(Guid id)
        {
            return await DbContext.NewsArticles
                .AsNoTracking()
                .SingleOrDefaultAsync(na => na.Id.Equals(id));
        }

        public async Task AddNewsArticleAsync(NewsArticle newsArticle)
        {
           await DbContext!.NewsArticles.AddAsync(newsArticle);
        }
    }
}
