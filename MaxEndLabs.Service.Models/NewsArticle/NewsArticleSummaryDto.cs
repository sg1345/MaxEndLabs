using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticleSummaryDto
    {
        public Guid Id { get; set; }
        public string TeaserTitle { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
    }
}
