using MaxEndLabs.Service.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxEndLabs.Service.Models.NewsArticle
{
    public class NewsArticlePaginationDto
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<NewsArticleSummaryDto> Articles { get; set; } = [];
    }
}
