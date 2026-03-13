namespace MaxEndLabs.Service.Models.Product
{
	public class ProductVariantDto
	{
		public int? Id { get; set; }
		public string VariantName { get; set; } = null!;
		public decimal? Price { get; set; }
	}
}
