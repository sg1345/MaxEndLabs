using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.Services.Core.Models.Configuration;
using MaxEndLabs.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static MaxEndLabs.Web.Common.PaginationConstants;

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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int page = 1)
        {
            string userId = GetAdminUserId()!;

            var orderDto = await _orderService.GetOrdersForUserAsync(userId, page, PageSizeOrderManager);

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

            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return PartialView("_OrderList", model);
            //}

            ViewBag.CurrentPage = page;
            return View(model);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrderManager(string searchTerm, string searchType, int page = 1)
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

            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    return PartialView("_OrderAdminList", model);
            //}

            ViewBag.CurrentPage = page;
			return View("Index", model);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Details(int orderId)
        {
            var dto = await _orderService.GetOrderDetailsAsync(orderId);

            var model = new OrderDetailsViewModel
            {
                OwnerUserId = dto.OwnderUserId,
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

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        //[ValidateAntiForgeryToken]
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
            catch (Exception e)
            {
                return NotFound(e);
            }

        }

    }
}
