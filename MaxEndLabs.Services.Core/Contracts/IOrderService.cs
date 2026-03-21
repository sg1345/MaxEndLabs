using MaxEndLabs.Service.Models.Order;

namespace MaxEndLabs.Services.Core.Contracts
{
	public interface IOrderService
    {
        Task<OrderCreateDto> GetOrderCreateDtoAsync(string userId);
        Task<int> CreateOrderAsync(OrderCreateDto dto);
    }
}
