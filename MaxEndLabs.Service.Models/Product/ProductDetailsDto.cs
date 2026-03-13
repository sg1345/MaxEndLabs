namespace MaxEndLabs.Service.Models.Product
{
	public class ProductDetailsDto : ProductDto
	{
		public string? Description { get; set; }
		public IEnumerable<ProductVariantDto> ProductVariants { get; set; } = [];
	}
}
