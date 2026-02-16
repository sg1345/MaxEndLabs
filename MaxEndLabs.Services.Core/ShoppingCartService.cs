using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxEndLabs.Data;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;

namespace MaxEndLabs.Services.Core
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly MaxEndLabsDbContext _context;

		public ShoppingCartService(MaxEndLabsDbContext context)
		{
			_context = context;
		}

		public Task<IEnumerable<ShoppingCartIndexViewModel>> GetShoppingCartAsync()
		{
			throw new NotImplementedException();
		}

		public Task AddProductToShoppingCartAsync()
		{
			throw new NotImplementedException();
		}

		public Task RemoveProductFromShoppingCartAsync()
		{
			throw new NotImplementedException();
		}

		public Task DeleteAllProductsFromShoppingCartAsync()
		{
			throw new NotImplementedException();
		}
	}
}
