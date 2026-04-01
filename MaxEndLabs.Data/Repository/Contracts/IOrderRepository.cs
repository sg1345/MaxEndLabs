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
		Task<IEnumerable<Order>?> GetSearchOrdersAsync(string searchType, string? searchTerm, int skip, int take);
		Task<int> GetCountAsync(string userId);
		Task<int> GetCountAsync(string searchType, string? searchTerm);
		Task<Order?> GetOrderByIdAsync(int id, bool isFiltered, bool includeOrderItem, bool includeUser);
		Task AddOrderAsync(Order order);
        Task<int> SaveChangesAsync();
        void UpdateOrder(Order order);
	}
}
