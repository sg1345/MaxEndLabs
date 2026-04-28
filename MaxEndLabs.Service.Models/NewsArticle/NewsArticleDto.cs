using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticleDto : NewsArticleDetailsDto
    {
        public DateTime PublishedAt { get; set; }
        public bool IsPublished { get; set; }
    }
}
