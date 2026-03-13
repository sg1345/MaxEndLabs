namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class ShoppingCartIndexDto
	{
		public decimal TotalPrice { get; set; }
		public int CartId { get; set; }
		public List<CartItemDto> CartItems { get; set; } = [];
	}
}
