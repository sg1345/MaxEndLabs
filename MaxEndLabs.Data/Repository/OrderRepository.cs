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

		public async Task<IEnumerable<Order>> GetPageOrdersAsync(Guid userId, int skip, int take)
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

		public async Task<IEnumerable<Order>?> GetSearchOrdersAsync(string searchType ,string? searchTerm, int skip, int take)
		{
			IQueryable<Order> query = DbContext.Orders
				.AsNoTracking()
				.OrderByDescending(o => o.CreatedAt)
				.ThenBy(o => o.Status)
				.ThenByDescending(o => o.UpdatedAt);

			if (searchType == "Username" && searchTerm != null)
			{
				return await query
					.Where(o => o.User.UserName!.Contains(searchTerm))
					.Skip(skip)
					.Take(take)
					.ToArrayAsync();
			}
			else if (searchType == "OrderNumber" && searchTerm != null)
			{
				return await query
					.Where(o => o.OrderNumber.Contains(searchTerm))
					.Skip(skip)
					.Take(take)
					.ToArrayAsync();
			}

			return null;

		}

		public async Task<int> GetCountAsync(Guid userId)
		{
			return await DbContext.Orders
				.AsNoTracking()
				.CountAsync(o => o.UserId == userId);
		}

		public async Task<int> GetCountAsync(string searchType, string? searchTerm)
		{
			IQueryable<Order> query = DbContext.Orders
				.AsNoTracking();

			if (searchType == "Username" && searchTerm != null)
			{
				return await query
					.CountAsync(o => o.User.UserName!.Contains(searchTerm));
			}
			else if (searchType == "OrderNumber" && searchTerm != null)
			{
				return await query
					.CountAsync(o => o.OrderNumber.Contains(searchTerm));
			}

			return 0;
		}

		public async Task<Order?> GetOrderByIdAsync(Guid id, bool isFiltered,  bool includeOrderItems, bool includeUser)
		{
			IQueryable<Order> query = DbContext.Orders
				.AsNoTracking();

			if (!isFiltered)
			{
				query = query.IgnoreQueryFilters();
			}

			if (includeOrderItems)
			{
				query = query
					.Include(o => o.OrderItems)
					.ThenInclude(oi => oi.ProductVariant)
					.Include(o => o.OrderItems)
					.ThenInclude(oi => oi.Product);
			}

			if (includeUser)
			{
				query = query.Include(o => o.User);
			}

			return await query
				.SingleOrDefaultAsync(o => o.Id == id);
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
