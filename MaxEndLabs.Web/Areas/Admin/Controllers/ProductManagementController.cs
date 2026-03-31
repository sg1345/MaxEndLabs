using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
