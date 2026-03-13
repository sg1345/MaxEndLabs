namespace MaxEndLabs.ViewModels.ShoppingCart
{
    public class ShoppingCartIndexViewModel
    {
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    }
}
