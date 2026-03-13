namespace MaxEndLabs.ViewModels.Product
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public decimal Price { get; set; }
        public string? MainImageUrl { get; set; }
        public string CategorySlug { get; set; } = null!;
    }
}
