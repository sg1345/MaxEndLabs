using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticleDetailsDto : NewsArticleSummaryDto
    {
        public string ContentTitle { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ArticleImageUrl { get; set; }
    }
}
