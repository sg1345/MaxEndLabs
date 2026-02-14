using MaxEndLabs.Data;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categories = await _productService.GetAllCategoriesAsync();

			return View(categories);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            return View("ProductsPage", products);
        }

        [AllowAnonymous]
        [Route("Products/ByCategory/{slug}")]
        public async Task<IActionResult> ByCategory(string slug)
        {

	        var products = await _productService.GetProductsByCategoryAsync(slug);

            if(products == null)
				return NotFound();
			

			return View("ProductsPage", products);
        }

        [AllowAnonymous]
		[Route("Products/ByCategory/{categorySlug}/{productSlug}")]
		public async Task<IActionResult> Details(string categorySlug, string productSlug)
        {
            var productDetails = await _productService.GetProductDetailsAsync(categorySlug, productSlug);
			return  View(productDetails);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ProductCreateViewModel model = await _productService.GetProductCreateViewModelAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model = await _productService.GetProductCreateViewModelAsync();
                return View(model);
            }

            int productId = await _productService.CreateProductAsync(model);

            return RedirectToAction("VariantManager", new { productId = productId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VariantManager(int productId)
        {
            ManageVariantsViewModel model = await _productService.GetProductByIdAsync(productId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VariantManager(ManageVariantsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _productService.GetProductByIdAsync(model.ProductId);

                return View(model);
            }

            await _productService.ManageProductVariantsAsync(model);

            return RedirectToAction("Details", new {categorySlug = model.CategorySlug, productSlug = model.ProductSlug});
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
