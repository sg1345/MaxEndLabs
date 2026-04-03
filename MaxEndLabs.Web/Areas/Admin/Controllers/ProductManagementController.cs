using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels.Category;
using MaxEndLabs.ViewModels.Product;

using Microsoft.AspNetCore.Mvc;
using static MaxEndLabs.Web.Common.PaginationConstants;
using static MaxEndLabs.GCommon.OutputMessages.Product;
using static MaxEndLabs.GCommon.ApplicationConstants;

namespace MaxEndLabs.Web.Areas.Admin.Controllers
{
	public class ProductManagementController : HomeController
	{
		private readonly IProductService _productService;

		public ProductManagementController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string searchTerm = "", int page = 1)
		{
			try
			{
				var productDto = await _productService.GetProductSearchAsync(searchTerm, page, PageSizeProductManager);

				var model = new ProductPaginationViewModel
				{
					SearchTerm = searchTerm,
					CurrentPage = productDto.CurrentPage,
					TotalPages = productDto.TotalPages,
					HasNextPage = productDto.HasNextPage,
					HasPreviousPage = productDto.HasPreviousPage,
					Products = productDto.Products.Select(p => new ProductPaginationEntityViewModel
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.Price,
						Slug = p.Slug,
						MainImageUrl = p.MainImageUrl,
						IsPublished = p.IsPublished,
						CategoryName = p.CategoryName,
						CategorySlug = p.CategorySlug
					}).ToList()
				};

				ViewBag.CurrentPage = page;
				return View(model);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			
		}

		[HttpGet]
		[Route("Admin/ProductManagement/{categorySlug}/{productSlug}")]
		public async Task<IActionResult> Details(string categorySlug, string productSlug)
		{
			try
			{
				var productDetailsDto = await _productService.GetProductDetailsAsync(productSlug, isFiltered: false);

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
				ViewBag.IsPublished = false;
				return View(productDetails);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch(InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }
        }

		[HttpGet]
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
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductFormViewModel model)
		{

			if (await _productService.ProductExistsAsync(model.Name))
			{
				ModelState.AddModelError("Name", "Product with this name Already exists.");
			}


			try
			{
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

				var productCreateDto = new ProductCreateDto()
				{
					Name = model.Name,
					Description = model.Description,
					Price = model.Price,
					CategoryId = model.CategoryId,
					MainImageUrl = model.MainImageUrl,
				};

				string productSlug = await _productService.AddProductAsync(productCreateDto);

				TempData[SuccessTempDataKey] = ProductCreated;
				return RedirectToAction("VariantManager", new { productSlug = productSlug });
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[ErrorTempDataKey] = FailedToCreateProduct;
				return View("Index","ProductManagement");
			}
		}

		[HttpGet]
		[Route("Admin/ProductManagement/VariantManager/{productSlug}")]
		public async Task<IActionResult> VariantManager(string productSlug)
		{
			try
			{
				var productVariantListDto = await _productService.GetProductAsync(productSlug, isFiltered: false);

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
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
            catch (InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }

        }

		[HttpPost]
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

				TempData[SuccessTempDataKey] = VariantUpdated;
				return RedirectToAction("Details", new
				{
					categorySlug = model.CategorySlug,
					productSlug = model.ProductSlug
				});
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[WarningTempDataKey] = NoChangesWereMade;
				return View(model);
			}

		}

		[HttpGet]
		[Route("Admin/ProductManagement/Edit/{productSlug}")]
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
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
            catch (InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }

        }

		[HttpPost]
		public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
		{
			if (await _productService.ProductExistsAsync(model.Name, model.Id))
			{
				ModelState.AddModelError("Name", "Product with this name Already exists.");
			}

			try
			{
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

				TempData[SuccessTempDataKey] = ProductEdited;
				return RedirectToAction("Details", new
				{
					categorySlug = result.CategorySlug,
					productSlug = result.ProductSlug
				});
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[WarningTempDataKey] = NoChangesWereMade;
				return RedirectToAction("Index", "Home");
			}
            catch (InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }
        }

		//We have onclick confirmation for delete, so I am skipping the GET method
		[HttpPost]
		public async Task<IActionResult> Delete(string productSlug)
		{
			try
			{
				await _productService.SoftDeleteProductAsync(productSlug);

				TempData[SuccessTempDataKey] = ProductRemoved;
				return RedirectToAction("Index");
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[ErrorTempDataKey] = FailedToDeleteProduct;
				return RedirectToAction("Index");
			}
            catch (InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }
        }

		public async Task<IActionResult> Restore(string productSlug)
		{
			try
			{
				await _productService.RestoreProductAsync(productSlug);

				TempData[SuccessTempDataKey] = ProductRestored;
				return RedirectToAction("Index");
			}
			catch (EntityNotFoundException e)
			{
				return NotFound();
			}
			catch (EntityPersistFailureException e)
			{
				TempData[ErrorTempDataKey] = FailedToRestoreProduct;
				return RedirectToAction("Index");
			}
            catch (InvalidOperationException e)
            {
                TempData[ErrorTempDataKey] = ServerError;
                return View("Index", "Home");
            }
        }
	}
}
