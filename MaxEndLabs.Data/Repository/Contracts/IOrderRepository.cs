using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data.Models;

namespace MaxEndLabs.Data.Repository.Contracts
{
	public interface IOrderRepository
	{
		Task<IEnumerable<Order>> GetPageOrdersAsync(string userId, int skip, int take);
		Task<int> GetCountAsync(string userId);
		Task<Order?> GetOrderByIdAsync(int id);
		Task AddOrderAsync(Order order);
        Task<int> SaveChangesAsync();
        void UpdateOrder(Order order);
	}
}
