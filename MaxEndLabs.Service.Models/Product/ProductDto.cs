namespace MaxEndLabs.Service.Models.Product
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Slug { get; set; } = null!;
		public decimal Price { get; set; }
		public string? MainImageUrl { get; set; }
		public string CategorySlug { get; set; } = null!;
	}
}
