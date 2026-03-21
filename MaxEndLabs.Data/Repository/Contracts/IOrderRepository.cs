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
        Task AddOrderAsync(Order order);
        Task<int> SaveChangesAsync();
    }
}
