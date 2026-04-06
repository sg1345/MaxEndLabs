namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class CartItemDto
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public Guid ProductVariantId { get; set; }
		public string VariantName { get; set; } = null!;
		public decimal UnitPrice { get; set; }
		public string? MainImageUrl { get; set; }
		public int Quantity { get; set; }
	}
}
