using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public async Task<int> GetCountAsync(string userId)
		{
			return await DbContext.Orders
				.AsNoTracking()
				.CountAsync(o => o.UserId == userId);
		}

		public async Task<Order?> GetOrderByIdAsync(int id)
		{
			return await DbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
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
