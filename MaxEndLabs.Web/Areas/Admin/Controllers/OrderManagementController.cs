using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using static MaxEndLabs.Web.Common.PaginationConstants;
using static MaxEndLabs.GCommon.ApplicationConstants;
using static MaxEndLabs.GCommon.OutputMessages.Order;

namespace MaxEndLabs.Web.Areas.Admin.Controllers
{
	public class OrderManagementController : HomeController
	{

		private readonly IOrderService _orderService;
		public OrderManagementController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string searchTerm = "", string searchType = DefaultSearchType, int page = 1)
		{
			try
			{
				var orderDto = await _orderService.GetOrderSearchAsync(searchTerm, searchType, page, PageSizeOrderManager);

				var model = new OrderPaginationViewModel
				{
					SearchTerm = searchTerm,
					SearchType = searchType,
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

				ViewBag.CurrentPage = page;
				return View(model);
			}
			catch (EntityNotFoundException e)
			{
				return View(new OrderPaginationViewModel());
			}
		}

		[HttpGet]
		public async Task<IActionResult> Details(int orderId)
		{
			try
			{
				var dto = await _orderService.GetOrderDetailsAsync(orderId);

				var model = new OrderDetailsViewModel
				{
					OwnerUserId = dto.OwnerUserId,
					OwnerFullName = dto.OwnerFullName,
					OwnerUsername = dto.OwnerUsername,
					CreatedAt = dto.CreatedAt,
					StatusBadgeClass = dto.StatusBadge,
					Status = dto.Status,
					StreetAddress = dto.StreetAddress,
					City = dto.City,
					Postcode = dto.Postcode,
					TotalAmount = dto.TotalAmount,
					OrderId = dto.OrderId,
					OrderNumber = dto.OrderNumber,
					Statuses = dto.Statuses,
					OrderItems = dto.LineItems.Select(li => new OrderItemViewModel
					{
						ProductName = li.ProductName,
						VariantName = li.VariantName,
						Quantity = li.Quantity,
						Price = li.Price,
						ImageUrl = li.ImageUrl,
						LineTotal = li.LineTotal
					})
						.ToList()
				};
				return PartialView("_OrderDetailsAdminPartial", model);
			}
			catch (EntityNotFoundException e)
			{
				return PartialView("NotFound");
			}
			catch (BadRequestException e)
			{
				return PartialView("BadRequest");
			}
            catch (InvalidOperationException e)
            {
				//I need another partial
                return PartialView("BadRequest");
            }
        }

		[HttpPost]
		public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
		{
            try
            {
                var orderStatus = await _orderService.GetOrderStatusAsync(orderId);

                if (orderStatus == newStatus)
                {
                    return await Details(orderId);
                }

                await _orderService.ChangeOrderStatus(newStatus, orderId);

                return await Details(orderId);

            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (BadRequestException e)
            {
                return BadRequest();

            }
            catch (EntityPersistFailureException e)
            {
                TempData[ErrorTempDataKey] = FailedToUpdateStatus;
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return RedirectToAction("Index", "Home");
            }
		}
	}
}
