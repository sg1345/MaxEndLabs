namespace MaxEndLabs.ViewModels.ShoppingCart
{
    public class CartItemRemoveViewModel
    {
        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }
        public Guid CartId { get; set; }
    }
}
