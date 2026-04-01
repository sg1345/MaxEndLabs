using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MaxEndLabs.Data.Models;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using static MaxEndLabs.Web.Common.PaginationConstants;

namespace MaxEndLabs.Web.ViewComponents
{
	public class OrderBoxViewComponent : ViewComponent
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IOrderService _orderService;

		public OrderBoxViewComponent(IOrderService orderService, UserManager<ApplicationUser> userManager)
		{
			_orderService = orderService;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync(int page = 1)
		{
			var userId = _userManager.GetUserId((ClaimsPrincipal)User);

			if (string.IsNullOrEmpty(userId))
			{
				return View(new OrderPaginationViewModel());
			}

			try
			{
				var orderDto = await _orderService.GetOrdersForUserAsync(userId, page, PageSizeHomePage);

				var model = new OrderPaginationViewModel
				{
					CurrentPage = orderDto.CurrentPage,
					TotalPages = orderDto.TotalPages,
					HasNextPage = orderDto.HasNextPage,
					HasPreviousPage = orderDto.HasPreviousPage,
					Orders = orderDto.Orders.Select(o => new OrderViewModel
					{
						Id = o.Id,
						OrderNumber = o.OrderNumber,
						TotalAmount = o.TotalAmount,
						Status = o.Status,
						CreatedAt = o.CreatedAt
					}).ToList()
				};

				return View(model);
			}
			catch (EntityNotFoundException e)
			{
				return View(new OrderPaginationViewModel());
			}
		}
	}
}
