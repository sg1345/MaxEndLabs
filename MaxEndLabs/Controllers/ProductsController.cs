using MaxEndLabs.Data;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MaxEndLabsDbContext _context;
        private readonly IProductService _productService;

		public ProductsController(MaxEndLabsDbContext context, IProductService productService)
        {
            _context = context;
			_productService = productService;
		}

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            var categories = await _productService.GetAllCategoriesAsync();

			return View(categories);
        }

        public async Task<IActionResult> AllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            return View("ProductsPage", products);
        }

        [Route("Products/ByCategory/{slug}")]
        public async Task<IActionResult> ByCategory(string slug)
        {

	        var products = await _productService.GetProductsByCategoryAsync(slug);

            if(products == null)
				return NotFound();
			

			return View("ProductsPage", products);
        }


		[Route("Products/ByCategory/{categorySlug}/{productSlug}")]
		public async Task<IActionResult> Details(string categorySlug, string productSlug)
        {
            var productDetails = await _productService.GetProductDetailsAsync(categorySlug, productSlug);
			return  View(productDetails);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return Ok("Create works");
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Ok();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return Ok("Edit works");
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Ok();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return Ok("Delete works");
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Ok();
            }
        }

    }
}
