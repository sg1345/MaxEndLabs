using MaxEndLabs.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.ViewModels.NewsArticles
{
    public class NewsArticlePaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<NewsArticleSummaryViewModel> Articles { get; set; } = [];

        public string? SearchTerm { get; set; }
    }
}
