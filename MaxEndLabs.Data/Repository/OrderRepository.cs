using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Data.Repository
{
	public class OrderRepository : BaseRepository, IOrderRepository
	{
		public OrderRepository(MaxEndLabsDbContext dbContext) 
			: base(dbContext)
		{
		}

		public async Task<IEnumerable<Order>> GetPageOrdersAsync(string userId, int skip, int take)
		{
			return await DbContext.Orders
				.AsNoTracking()
				.Where(o => o.UserId == userId)
				.OrderByDescending(o => o.CreatedAt)
				.ThenBy(o => o.Status)
				.ThenByDescending(o=>o.UpdatedAt)
                .Skip(skip) 
                .Take(take) 
                .ToArrayAsync();
		}

		public async Task<IEnumerable<Order>?> GetSearchOrdersAsync(string searchType ,string searchTerm, int skip, int take)
		{
			if (searchType == "Username")
			{
				return await DbContext.Orders
					.AsNoTracking()
					.Where(o => o.User.UserName.Contains(searchTerm))
					.OrderByDescending(o => o.CreatedAt)
					.ThenBy(o => o.Status)
					.ThenByDescending(o => o.UpdatedAt)
					.Skip(skip)
					.Take(take)
					.ToArrayAsync();
			}
			else if (searchType == "OrderNumber")
			{
				return await DbContext.Orders
					.AsNoTracking()
					.Where(o => o.OrderNumber.Contains(searchTerm))
					.OrderByDescending(o => o.CreatedAt)
					.ThenBy(o => o.Status)
					.ThenByDescending(o => o.UpdatedAt)
					.Skip(skip)
					.Take(take)
					.ToArrayAsync();
			}

			return null;

		}

		public async Task<int> GetCountAsync(string userId)
		{
			return await DbContext.Orders
				.AsNoTracking()
				.CountAsync(o => o.UserId == userId);
		}

		public async Task<int> GetCountAsync(string searchType, string searchTerm)
		{
			if (searchType == "Username")
			{
				return await DbContext.Orders
					.AsNoTracking()
					.CountAsync(o => o.User.UserName.Contains(searchTerm));
			}
			else // if (searchType == "OrderNumber")
			{
				return await DbContext.Orders
					.AsNoTracking()
					.CountAsync(o => o.OrderNumber.Contains(searchTerm));
			}
		}

		public async Task<Order?> GetOrderByIdAsync(int id)
		{
			return await DbContext.Orders
				.IgnoreQueryFilters()
				.Include(o=> o.OrderItems)
				.ThenInclude(oi=>oi.ProductVariant)
				.Include(o=>o.OrderItems)
				.ThenInclude(oi => oi.Product)
                .Include(o=> o.User)
				.FirstOrDefaultAsync(o => o.Id == id);
		}

		public async Task AddOrderAsync(Order order)
        {
            await DbContext.Orders.AddAsync(order);
        }

		public void UpdateOrder(Order order)
		{
			DbContext.Orders.Update(order);
		}
	}
}
