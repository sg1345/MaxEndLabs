namespace MaxEndLabs.Service.Models.Product
{
	public class ProductCreateDto
	{
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public string? MainImageUrl { get; set; }
		public int CategoryId { get; set; }
		public string? Description { get; set; }
	}
}
