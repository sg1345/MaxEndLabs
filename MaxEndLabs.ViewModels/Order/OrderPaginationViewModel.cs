namespace MaxEndLabs.ViewModels.Order
{
	public class OrderPaginationViewModel
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public bool HasPreviousPage { get; set; }
		public bool HasNextPage { get; set; }
		public IEnumerable<OrderViewModel> Orders { get; set; } = [];

		public string? SearchType { get; set; }
		public string? SearchTerm { get; set; }
	}
}
