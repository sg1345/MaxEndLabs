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

        public async Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync()
        {
           return await DbContext.NewsArticles
                .AsNoTracking()
                .ToListAsync();
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
