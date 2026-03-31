using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
