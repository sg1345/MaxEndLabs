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

        public async Task<StripeSessionDto> CreateOrderAsync(AddressOrderDto dto)
        {
            string orderNumber = GenerateOrderNumber();

            var cartItemList = await _shoppingCartRepository.GetCartItemsByUserIdAsync(dto.UserId);

            decimal totalPrice = cartItemList.Sum(ci => ci.Quantity*(ci.ProductVariant.Price ?? ci.Product.Price));

            var order = new Order
            {
                UserId = dto.UserId,
                OrderNumber = orderNumber,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                Postcode = dto.Postcode,
                Status = OrderStatus.Pending,
                TotalAmount = totalPrice,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OrderItems = cartItemList.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        ProductVariantId = ci.ProductVariantId,
                        UnitPrice = ci.ProductVariant.Price ?? ci.Product.Price,
                        Quantity = ci.Quantity,
                        LineTotal = (ci.ProductVariant.Price ?? ci.Product.Price) * ci.Quantity
                    })
                    .ToList()
            };

            await _orderRepository.AddOrderAsync(order);
            await EnsureSaveChangesAsync();

            int cartId = order.Id;
            var lineItems = cartItemList.Select(ci => new StripeLineItemDto
			{
				ProductName = ci.Product.Name,
				VariantName = ci.ProductVariant.VariantName,
				Price = (long)((ci.ProductVariant?.Price ?? ci.Product.Price) * 100),
				Quantity = ci.Quantity,
				ImageUrl = ci.Product.MainImageUrl
			}).ToList();
			//int cartId = await _shoppingCartRepository.GetShoppingCartIdAsync(dto.UserId);
			var result = new StripeSessionDto
            {
	            OrderNumber = orderNumber,
	            CartId = cartItemList.FirstOrDefault()?.CartId ?? 0,
	            OrderId = order.Id,
	            LineItems = lineItems
            };

			return result;
		}

        public async Task<string> MarkOrderAsPaidAsync(int orderId)
        {
	        Order? order = _orderRepository.GetOrderByIdAsync(orderId).Result;

	        if (order == null) return null!;

			order.Status = OrderStatus.Paid;
			order.UpdatedAt = DateTime.UtcNow;

			_orderRepository.UpdateOrder(order);
			await EnsureSaveChangesAsync();

			return order.Status.ToString();
        }

        public async Task<string?> GetOrderStatusAsync(int orderId)
        {
	        Order? order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order == null)
                return null;

			return order.Status.ToString();
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
