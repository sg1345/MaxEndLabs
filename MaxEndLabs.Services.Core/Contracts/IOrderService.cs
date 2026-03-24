using MaxEndLabs.Service.Models.Order;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IOrderService
    {
        Task<OrderPaginationDto> GetOrdersForUserAsync(string userId, int page);
		Task<OrderCreateDto> GetOrderCreateDtoAsync(string userId);
        Task<StripeSessionDto> CreateOrderAsync(AddressOrderDto dto);
        Task<string> MarkOrderAsPaidAsync(int orderId);
		Task<string?> GetOrderStatusAsync(int orderId);
    }
}
