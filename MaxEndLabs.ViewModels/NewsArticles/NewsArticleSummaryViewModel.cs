using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.NewsArticles
{
    public class NewsArticleSummaryViewModel
    {
        public Guid Id { get; set; }
        public string TeaserTitle { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
    }
}
