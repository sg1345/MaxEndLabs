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

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult KarlosNasarStory()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult AboutUs()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
