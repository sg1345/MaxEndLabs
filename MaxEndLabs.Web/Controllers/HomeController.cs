using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MaxEndLabs.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MaxEndLabs.Web.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult GetOrderBox(int page)
		{
			return ViewComponent("OrderBox", new { page = page });
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Index()
		{
			if (User.IsInRole("Admin"))
			{
				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}

			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult KarlosNasarStory()
		{
			return View();
		}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Euro2026()
        {
            return View();
        }

        [HttpGet]
		[AllowAnonymous]
		public IActionResult AboutUs()
		{
			return View();
		}
		
		[Route("Home/Error/{statusCode}")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statusCode)
		{
			if (statusCode == StatusCodes.Status404NotFound)
			{
				return View("NotFound");
			}

			if (statusCode == StatusCodes.Status400BadRequest)
			{
				return View("BadRequest");
			}

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
