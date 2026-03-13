namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class CartItemCreateDto : CartItemDeleteDto
	{
		public int Quantity { get; set; }
	}
}
