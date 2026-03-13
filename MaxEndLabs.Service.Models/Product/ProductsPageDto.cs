namespace MaxEndLabs.Service.Models.Product
{
	public class ProductsPageDto
	{
		public string Title { get; set; } = null!;
		public IEnumerable<ProductDto> Products { get; set; } = [];
	}
}
