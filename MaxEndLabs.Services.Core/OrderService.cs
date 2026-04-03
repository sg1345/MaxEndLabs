using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Models.Enum;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
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

        public async Task<OrderPaginationDto> GetOrdersForUserAsync(string userId, int page, int pageSize)
		{
			int skip = (page - 1) * pageSize;
			var orders = await _orderRepository.GetPageOrdersAsync(userId, skip, pageSize);

			if(orders == null)
				throw new EntityNotFoundException();

			var count = await _orderRepository.GetCountAsync(userId);

            bool hasPreviousPage = page > 1;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new OrderPaginationDto
			{
				CurrentPage = page,
				TotalPages = totalPages,
				HasPreviousPage = hasPreviousPage,
				HasNextPage = page < totalPages,
                Orders = orders.Select(o => new OrderDto
				{
					Id = o.Id,
					OrderNumber = o.OrderNumber,
					TotalAmount = o.TotalAmount,
					Status = o.Status.ToString(),
					CreatedAt = DateOnly.FromDateTime(o.CreatedAt)
				}).ToList()
			};
		}

		public async Task<OrderPaginationDto> GetOrderSearchAsync
            (string searchTerm, string searchType, int page, int pageSize)
		{
			int skip = (page - 1) * pageSize;
			var orders = await _orderRepository.GetSearchOrdersAsync(searchType, searchTerm, skip, pageSize);

			if(orders == null)
				throw new EntityNotFoundException();

			var count = await _orderRepository.GetCountAsync(searchType, searchTerm);

            bool hasPreviousPage = page > 1;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new OrderPaginationDto
			{
				CurrentPage = page,
				TotalPages = totalPages,
				HasPreviousPage = hasPreviousPage,
				HasNextPage = page < totalPages,
				Orders = orders.Select(o => new OrderDto
				{
					Id = o.Id,
					OrderNumber = o.OrderNumber,
					TotalAmount = o.TotalAmount,
					Status = o.Status.ToString(),
					CreatedAt = DateOnly.FromDateTime(o.CreatedAt)
				}).ToList()
			};

		}

		public async Task<OrderCreateDto> GetOrderCreateDtoAsync(string userId)
		{
			int shoppingCartId = await _shoppingCartRepository.GetShoppingCartIdAsync(userId);

			var cartItemList = await _shoppingCartRepository.GetCartItemsByUserIdAsync(userId);

			if (cartItemList == null || shoppingCartId == 0)
				throw new EntityNotFoundException();

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

		public async Task<int> CreateOrderAsync(AddressOrderDto dto)
		{
			string orderNumber = GenerateOrderNumber();

			var cartItemList = await _shoppingCartRepository.GetCartItemsByUserIdAsync(dto.UserId);

			if (cartItemList == null)
				throw new EntityNotFoundException();

			decimal totalPrice = cartItemList.Sum(ci => ci.Quantity * (ci.ProductVariant.Price ?? ci.Product.Price));

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

			return order.Id;
		}

		public async Task<StripeSessionDto> GetOrderAsync(int orderId)
        {
            Order? order = await _orderRepository.GetOrderByIdAsync(orderId, isFiltered: false, includeOrderItem: true, includeUser: false);
            

			if (order == null) return null!;

			var stripeSessionDto = new StripeSessionDto
			{
				OrderId = order.Id,
				OrderNumber = order.OrderNumber,
				LineItems = order.OrderItems.Select(oi => new StripeOrderItemDto
				{
					ProductName = oi.Product.Name,
					VariantName = oi.ProductVariant.VariantName,
					Price = (long)(oi.UnitPrice * 100),
					Quantity = oi.Quantity,
					ImageUrl = oi.Product.MainImageUrl
				})
					.ToList()
			};

			return stripeSessionDto;
		}

		public async Task<OrderDetailsDto> GetOrderDetailsAsync(int orderId)
        {
            Order? order = await _orderRepository.GetOrderByIdAsync(orderId, isFiltered: false, includeOrderItem: true, includeUser: true);
			
			if (order == null)
				throw new EntityNotFoundException();

			string statusBadge = order.Status switch
			{
				OrderStatus.Pending => "bg-warning-subtle text-warning-emphasis border border-warning-subtle",
				OrderStatus.Paid => "bg-info-subtle text-info-emphasis border border-info-subtle",
				OrderStatus.Shipped => "bg-primary-subtle text-primary-emphasis border border-primary-subtle",
				OrderStatus.Completed => "bg-success-subtle text-success-emphasis border border-success-subtle",
				OrderStatus.Cancelled => "badge bg-danger-subtle text-danger-emphasis border border-danger-subtle",
				OrderStatus.Refunded => "badge bg-danger-subtle text-danger-emphasis border border-danger-subtle",
				_ => ""
			};

			var statuses = Enum.GetNames(typeof(OrderStatus)).ToList();

			if (!statuses.Any())
				throw new BadRequestException();

			var orderDetailsDto = new OrderDetailsDto
			{
				OwnderUserId = order.UserId,
				OrderId = order.Id,
				OwnerFullName = order.User.FullName,
				OwnerUsername = order.User.UserName!,
				OrderNumber = order.OrderNumber,
				StreetAddress = order.StreetAddress,
				City = order.City,
				Postcode = order.Postcode,
				StatusBadge = statusBadge,
				Status = order.Status.ToString(),
				TotalAmount = order.TotalAmount,
				CreatedAt = order.CreatedAt,
				Statuses = statuses,
				LineItems = order.OrderItems.Select(oi => new OrderItemDetailsDto()
				{
					ProductName = oi.Product.Name,
					VariantName = oi.ProductVariant.VariantName,
					Quantity = oi.Quantity,
					Price = oi.UnitPrice,
					ImageUrl = oi.Product.MainImageUrl,
					LineTotal = oi.LineTotal
				})
					.ToList()
			};

			return orderDetailsDto;
		}

		public async Task<string> MarkOrderAsPaidAsync(int orderId)
		{
			Order? order = await _orderRepository.GetOrderByIdAsync(orderId, isFiltered: true, includeOrderItem: false, includeUser: false);
            
			if (order == null)
				throw new EntityNotFoundException();

			order.Status = OrderStatus.Paid;
			order.UpdatedAt = DateTime.UtcNow;

			_orderRepository.UpdateOrder(order);
			await EnsureSaveChangesAsync();

			return order.Status.ToString();
		}

		public async Task<string?> GetOrderStatusAsync(int orderId)
		{
			Order? order = await _orderRepository.GetOrderByIdAsync(orderId, isFiltered: true, includeOrderItem: false, includeUser: false);

			if (order == null)
				throw new EntityNotFoundException();

			return order.Status.ToString();
		}

		public async Task ChangeOrderStatus(string status, int orderId)
		{
			Order? order = await _orderRepository.GetOrderByIdAsync(orderId, isFiltered: true, includeOrderItem: false, includeUser: false);

			if (order == null)
				throw new EntityNotFoundException();

			if (!Enum.TryParse(status, true, out OrderStatus newStatus))
			{
				throw new BadRequestException();
			}

			order.Status = newStatus;
			order.UpdatedAt = DateTime.UtcNow;

			_orderRepository.UpdateOrder(order);

			await EnsureSaveChangesAsync();
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
				throw new EntityPersistFailureException("Database Operation Failed");
			}
		}
	}
}
