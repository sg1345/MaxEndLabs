using Microsoft.EntityFrameworkCore;
using MaxEndLabs.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaxEndLabs.Data.Configuration
{
    internal class NewsArticleConfiguration : IEntityTypeConfiguration<NewsArticle>
    {
        public void Configure(EntityTypeBuilder<NewsArticle> entity)
        {
            entity
                .HasQueryFilter(na => na.IsPublished == true);
        }
    }
}
