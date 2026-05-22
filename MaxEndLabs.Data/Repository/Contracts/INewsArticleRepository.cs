using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>?> GetNewsArticlesSearchAsync(string? searchTerm, int skip, int take, bool isFiltered);
        Task<int> GetCountAsync(string? searchTerm);
        Task<NewsArticle?> GetNewsArticleByIdAsync(Guid id);
        Task AddNewsArticleAsync(NewsArticle newsArticle);
        void UpdateNewsArticleAsync(NewsArticle newsArticle);
        Task<int> SaveChangesAsync();
    }
}
