using MaxEndLabs.Service.Models.Order;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IOrderService
    {
        Task<OrderPaginationDto> GetOrdersForUserAsync(string userId, int page, int pageSize);
        Task<OrderPaginationDto> GetOrderSearchAsync(string searchTerm, string searchType, int page, int pageSize);
		Task<OrderCreateDto>GetOrderCreateDtoAsync(string userId);
        Task<int>CreateOrderAsync(AddressOrderDto dto);
        Task<StripeSessionDto> GetOrderAsync(int orderId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int orderId);
        Task<string> MarkOrderAsPaidAsync(int orderId);
		Task<string?> GetOrderStatusAsync(int orderId);

		Task ChangeOrderStatus(string status, int orderId);


    }
}
