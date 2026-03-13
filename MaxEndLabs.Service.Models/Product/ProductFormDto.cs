using MaxEndLabs.Service.Models.Category;

namespace MaxEndLabs.Service.Models.Product
{
    public class ProductFormDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public int CategoryId { get; set; }
		public decimal Price { get; set; }
		public string? MainImageUrl { get; set; }

		public IEnumerable<CategorySelectDto> Categories { get; set; } = [];
	}
}
