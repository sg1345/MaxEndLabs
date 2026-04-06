using MaxEndLabs.Service.Models.Order;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IOrderService
    {
        Task<OrderPaginationDto> GetOrdersForUserAsync(Guid userId, int page, int pageSize);
        Task<OrderPaginationDto> GetOrderSearchAsync(string searchTerm, string searchType, int page, int pageSize);
		Task<OrderCreateDto>GetOrderCreateDtoAsync(Guid userId);
        Task<Guid>CreateOrderAsync(AddressOrderDto dto);
        Task<StripeSessionDto> GetOrderAsync(Guid orderId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId);
        Task<string> MarkOrderAsPaidAsync(Guid orderId);
		Task<string?> GetOrderStatusAsync(Guid orderId);

		Task ChangeOrderStatus(string status, Guid orderId);


    }
}
