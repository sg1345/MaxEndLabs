using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;

using MaxEndLabs.Services.Core.Contracts;

using MaxEndLabs.ViewModels.Category;
using MaxEndLabs.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxEndLabs.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

		public ProductsController(IProductService productService)
        {
			_productService = productService;
		}

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categoryDtoList = await _productService.GetAllCategoriesAsync();

			var categories = categoryDtoList
				.Select(c => new ProductIndexViewModel()
				{
					Id = c.Id,
					Name = c.Name,
					Slug = c.Slug
				})
				.ToList();

			return View(categories);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AllProducts()
        {
            var productsPageDto = await _productService.GetAllProductsAsync();

            var products = new ProductsPageViewModel()
            {
	            Title = productsPageDto.Title,
	            Products = productsPageDto.Products
		            .Select(p => new ProductListViewModel()
		            {
			            Id = p.Id,
			            Name = p.Name,
			            Slug = p.Slug,
			            Price = p.Price,
			            MainImageUrl = p.MainImageUrl,
			            CategorySlug = p.CategorySlug
		            })
		            .ToList()
            };

            return View("ProductsPage", products);
        }

        [AllowAnonymous]
        [Route("Products/ByCategory/{slug}")]
        public async Task<IActionResult> ByCategory(string slug)
        {
                var productsPageDto = await _productService.GetProductsByCategoryAsync(slug);

                var products = new ProductsPageViewModel()
                {
	                Title = productsPageDto.Title,
	                Products = productsPageDto.Products
		                .Select(p => new ProductListViewModel()
		                {
			                Id = p.Id,
			                Name = p.Name,
			                Slug = p.Slug,
			                Price = p.Price,
			                MainImageUrl = p.MainImageUrl,
			                CategorySlug = p.CategorySlug
		                })
		                .ToList()
                };

                return View("ProductsPage", products);
        }

        [AllowAnonymous]
		[Route("Products/ByCategory/{categorySlug}/{productSlug}")]
		public async Task<IActionResult> Details(string categorySlug, string productSlug)
        {
            try
            {
                var productDetailsDto = await _productService.GetProductDetailsAsync(productSlug);

                var productDetails = new ProductDetailsViewModel()
				{
					Id = productDetailsDto.Id,
					Name = productDetailsDto.Name,
					ProductSlug = productDetailsDto.Slug,
					CategorySlug = productDetailsDto.CategorySlug,
					Description = productDetailsDto.Description,
					Price = productDetailsDto.Price,
					MainImageUrl = productDetailsDto.MainImageUrl,
					ProductVariants = productDetailsDto.ProductVariants
						.Select(v => new VariantDisplayViewModel()
						{
							Id = v.Id,
							VariantName = v.VariantName,
							Price = v.Price
						})
						.ToList()
				};

				return View(productDetails);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var productFormDto = await _productService.GetProductCreateDtoAsync();

                var model = new ProductFormViewModel()
                {
                    Categories = productFormDto.Categories.Select(c => new CategorySelectViewModel()
					{
						Id = c.Id,
						Name = c.Name
					})
						.ToList()
				};

                return View(model);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {

	        if (await _productService.ProductExistsAsync(model.Name))
	        {
		        ModelState.AddModelError("Name", "Product with this name Already exists.");
	        }

	        if (!ModelState.IsValid)
	        {
		        var categories = await _productService.GetAllCategoriesAsync();

		        model.Categories = categories
			        .Select(c => new CategorySelectViewModel()
			        {
				        Id = c.Id,
				        Name = c.Name
			        })
			        .ToList();

				return View(model);
	        }

			try
            {
                var productCreateDto = new ProductCreateDto()
				{
					Name = model.Name,
					Description = model.Description,
					Price = model.Price,
					CategoryId = model.CategoryId,
                    MainImageUrl = model.MainImageUrl,
				};

				string productSlug = await _productService.AddProductAsync(productCreateDto);

				TempData["SuccessMessage"] = "Product created!";
                return RedirectToAction("VariantManager", new { productSlug = productSlug });
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Products/VariantManager/{productSlug}")]
        public async Task<IActionResult> VariantManager(string productSlug)
        {
            try
            {
                var productVariantListDto = await _productService.GetProductAsync(productSlug);

                var model = new ManageVariantsViewModel()
				{
					ProductId = productVariantListDto.ProductId,
					ProductName = productVariantListDto.ProductName,
					ProductSlug = productVariantListDto.ProductSlug,
					CategorySlug = productVariantListDto.CategorySlug,
					Variants = productVariantListDto.Variants
						.Select(v => new VariantEditViewModel()
						{
							Id = v.Id,
							Name = v.VariantName,
							Price = v.Price
						})
						.ToList()
				};

                return View(model);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VariantManager(ManageVariantsViewModel model)
        {
	        if (!model.Variants.Any())
	        {
		        ModelState.AddModelError("Variants", "You must add at least one variant before finishing.");
	        }

	        if (!ModelState.IsValid)
	        {
		        return View(model);
	        }

			try
			{
				var productVariantListDto = new ProductVariantListDto()
				{
					ProductId = model.ProductId,
					ProductName = model.ProductName,
					ProductSlug = model.ProductSlug,
					CategorySlug = model.CategorySlug,
					Variants = model.Variants
						.Select(v => new ProductVariantDto()
						{
							Id = v.Id,
							VariantName = v.Name,
							Price = v.Price
						})
						.ToList()
				};

                await _productService.ManageProductVariantsAsync(productVariantListDto);

                TempData["SuccessMessage"] = "Variants updated!";
                return RedirectToAction("Details", new 
	                { 
		                categorySlug = model.CategorySlug, 
		                productSlug = model.ProductSlug
	                });
            }
            catch (ArgumentException e)
            {
	            TempData["WarningMessage"] = "No Changes were made!";
	            return View(model);
            }
	        
        }

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("Products/Edit/{productSlug}")]
		public async Task<IActionResult> Edit(string productSlug)
		{
            try
            {
                var productEditDto = await _productService.GetProductEditDtoAsync(productSlug);

                var model = new ProductFormViewModel()
                {
					Id = productEditDto.Id,
					Name = productEditDto.Name,
					Description = productEditDto.Description,
					Price = productEditDto.Price,
					CategoryId = productEditDto.CategoryId,
					MainImageUrl = productEditDto.MainImageUrl,
					Categories = productEditDto.Categories.Select(c => new CategorySelectViewModel()
						{
							Id = c.Id,
							Name = c.Name
						})
						.ToList()
				};


				return View(model);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }

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
				var categories = await _productService.GetAllCategoriesAsync();

				model.Categories = categories
					.Select(c => new CategorySelectViewModel()
					{
						Id = c.Id,
						Name = c.Name
					})
					.ToList();

				return View(model);
			}

			try
			{
				var dto = new ProductFormDto()
				{
					Id = model.Id,
					Name = model.Name,
					Description = model.Description,
					Price = model.Price,
					CategoryId = model.CategoryId,
					MainImageUrl = model.MainImageUrl,
					Categories = model.Categories
						.Select(c => new CategorySelectDto() 
						{
							Id = c.Id,
							Name = c.Name
						})
						.ToList()
				};

                (string CategorySlug, string ProductSlug) result = await _productService.EditProductAsync(dto);

				TempData["SuccessMessage"] = "Product edited!";
                return RedirectToAction("Details", new
                {
	                categorySlug = result.CategorySlug, 
	                productSlug = result.ProductSlug
                });
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
			

		}

		//We have onclick confirmation for delete, so I am skipping the GET method
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string productSlug)
        {

			try
			{
				await _productService.DeleteProductAsync(productSlug);

				TempData["SuccessMessage"] = "Product deleted!";
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
		}

    }
}
