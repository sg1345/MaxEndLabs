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
                var productDetailsDto = await _productService.GetProductDetailsAsync(productSlug, isFiltered: true);

                var productDetails = new ProductDetailsViewModel()
				{
					Id = productDetailsDto.Id,
					Name = productDetailsDto.Name,
					ProductSlug = productDetailsDto.Slug,
					CategorySlug = productDetailsDto.CategorySlug,
					Description = productDetailsDto.Description,
					Price = productDetailsDto.Price,
					MainImageUrl = productDetailsDto.MainImageUrl,
					IsPublished = productDetailsDto.IsPublished,
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
    }
}
