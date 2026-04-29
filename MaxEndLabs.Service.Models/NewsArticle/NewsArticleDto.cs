using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticleDto : NewsArticleDetailsDto
    {
        public string TeaserTitle { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
    }
}
