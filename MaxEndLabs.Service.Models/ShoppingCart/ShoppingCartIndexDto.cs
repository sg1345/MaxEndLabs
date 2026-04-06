namespace MaxEndLabs.Service.Models.ShoppingCart
{
	public class ShoppingCartIndexDto
	{
		public decimal TotalPrice { get; set; }
		public Guid CartId { get; set; }
		public List<CartItemDto> CartItems { get; set; } = [];
	}
}
