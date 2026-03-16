using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Controllers
{
	public class OrderController : BaseController
	{
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
