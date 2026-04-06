namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class CartItemDeleteDto
	{
		public Guid CartId { get; set; }
		public Guid ProductId { get; set; }
		public Guid ProductVariantId { get; set; }
	}
}
