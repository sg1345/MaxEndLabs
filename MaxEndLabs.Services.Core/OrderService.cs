using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Models.Enum;
using MaxEndLabs.Data.Repository;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core.Contracts;

namespace MaxEndLabs.Services.Core
{
	public class OrderService : IOrderService
	{
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<OrderCreateDto> GetOrderCreateDtoAsync(string userId)
        {
            int shoppingCartId = await _shoppingCartRepository.GetShoppingCartIdAsync(userId);

            var cartItemList = await _shoppingCartRepository.GetCartItemsByUserIdAsync(userId);

            var cartItemListDto = cartItemList
                .Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    ProductVariantId = ci.ProductVariant!.Id,
                    VariantName = ci.ProductVariant!.VariantName,
                    UnitPrice = ci.ProductVariant.Price ?? ci.Product.Price,
                    MainImageUrl = ci.Product.MainImageUrl,
                    Quantity = ci.Quantity
                })
                .ToList();

            var orderCreateDto = new OrderCreateDto
            {
                CartId = shoppingCartId,
                TotalPrice = cartItemListDto.Sum(item => item.UnitPrice * item.Quantity),
                CartItems = cartItemListDto
            };

            return orderCreateDto;

        }

        public async Task<int> CreateOrderAsync(OrderCreateDto dto)
        {
            string orderNumber = GenerateOrderNumber();

            var order = new Order
            {
                UserId = dto.UserId,
                OrderNumber = orderNumber,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                Postcode = dto.Postcode,
                Status = OrderStatus.Pending,
                TotalAmount = dto.TotalPrice,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderItems = dto.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        ProductVariantId = ci.ProductVariantId,
                        UnitPrice = ci.UnitPrice,
                        Quantity = ci.Quantity,
                        LineTotal = ci.UnitPrice * ci.Quantity
                    })
                    .ToList()
            };

            await _orderRepository.AddOrderAsync(order);
            await EnsureSaveChangesAsync();

            return await _shoppingCartRepository.GetShoppingCartIdAsync(dto.UserId);
        }

        private string GenerateOrderNumber()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();

            return $"ORD-{datePart}-{randomPart}";
        }

        private async Task EnsureSaveChangesAsync()
        {
            int changes = await _orderRepository.SaveChangesAsync();

            var successAdd = changes > 0;

            if (!successAdd)
            {
                throw new ArgumentException("Database Operation Failed");
            }
        }
    }
}
