using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Controllers
{
    
    public class NewsController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
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
    }
}
