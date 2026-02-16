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
        private readonly IProductService _productService;

		public ProductsController(IProductService productService)
        {
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
            ProductFormViewModel model = await _productService.GetProductCreateViewModelAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
	        if (await _productService.ProductExistsAsync(model.Name))
	        {
                ModelState.AddModelError("Name","Product with this name Already exists.");
	        }

            if (!ModelState.IsValid)
            {
                model = await _productService.GetProductCreateViewModelAsync();
                return View(model);
            }

            string productSlug = await _productService.AddProductAsync(model);

            return RedirectToAction("VariantManager", new { productSlug = productSlug });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Products/VariantManager/{productSlug}")]
        public async Task<IActionResult> VariantManager(string productSlug)
        {
            ManageVariantsViewModel model = await _productService.GetProductAsync(productSlug);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VariantManager(ManageVariantsViewModel model)
        {
	        if (model.Variants == null || !model.Variants.Any())
	        {
		        ModelState.AddModelError("Variants", "You must add at least one variant before finishing.");
	        }

			if (!ModelState.IsValid)
            {
                model = await _productService.GetProductAsync(model.ProductSlug);

                return View(model);
            }

            await _productService.ManageProductVariantsAsync(model);

            return RedirectToAction("Details", new {categorySlug = model.CategorySlug, productSlug = model.ProductSlug});
        }

		// GET: ProductController/Edit/5
		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("Products/Edit/{productSlug}")]
		public async Task<IActionResult> Edit(string productSlug)
		{
			ProductFormViewModel model = await _productService.GetProductEditViewModelAsync(productSlug);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
		{
			if (await _productService.ProductExistsAsync(model.Name, model.Id)) 
			{
				ModelState.AddModelError("Name", "Product with this name Already exists.");
			}

			if (!ModelState.IsValid)
			{
				model = await _productService.GetProductEditViewModelAsync(model.Id);

				return View(model);
			}

			(string CategorySlug, string ProductSlug) result = await _productService.EditProductAsync(model);
			return RedirectToAction("Details", new { categorySlug = result.CategorySlug, productSlug = result.ProductSlug });

		}

		//We have onclick confirmation for delete, so I am skipping the GET method
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string slug)
        {
            await _productService.DeleteProductAsync(slug);
			return RedirectToAction("Index");
		}

    }
}
