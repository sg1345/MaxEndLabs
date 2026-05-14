using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.NewsArticles
{
    public class NewsArticleDetailsViewModel
    {
        public Guid Id { get; set; }
        public string ContentTitle { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ArticleImageUrl { get; set; }
    }
}
