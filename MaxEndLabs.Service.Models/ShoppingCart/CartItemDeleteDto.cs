namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class CartItemDeleteDto
	{
		public int CartId { get; set; }
		public int ProductId { get; set; }
		public int ProductVariantId { get; set; }
	}
}
