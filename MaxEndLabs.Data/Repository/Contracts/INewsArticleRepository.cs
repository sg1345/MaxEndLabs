using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsArticlesAsync();
        Task<NewsArticle?> GetNewsArticleByIdAsync(Guid id);
        Task AddNewsArticleAsync(NewsArticle newsArticle);
        Task<int> SaveChangesAsync();
    }
}
