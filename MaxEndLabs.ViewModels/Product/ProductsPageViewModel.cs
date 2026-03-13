namespace MaxEndLabs.ViewModels.Product
{
    public class ProductsPageViewModel
    {
        public string Title { get; set; } = null!;
        public IEnumerable<ProductListViewModel> Products { get; set; } = [];

    }
}
