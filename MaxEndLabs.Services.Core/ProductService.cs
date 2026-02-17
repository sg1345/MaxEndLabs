using System.Text.RegularExpressions;
using MaxEndLabs.Data;
using MaxEndLabs.Data.Models;
using MaxEndLabs.Services.Core.Contracts;
using MaxEndLabs.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MaxEndLabs.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly MaxEndLabsDbContext _context;

        public ProductService(MaxEndLabsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProductExistsAsync(string productName, int productId)
        {
			var productSlug = GenerateSlug(productName);

			return await _context.Products
		        .AnyAsync(p => p.Slug == productSlug && p.Id != productId);
        }

        public async Task<bool> ProductExistsAsync(string productName)
        {
			var productSlug = GenerateSlug(productName);

			return await _context.Products
				.AnyAsync(p => p.Slug == productSlug);
		}

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.Name)
                .Select(c => new ProductIndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug
                })
                .ToListAsync();
        }

        public async Task<ProductsPageViewModel> GetAllProductsAsync()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsPublished)
                .OrderBy(p => p.Name)
                .Select(p => new ProductListViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Price = p.Price,
                    MainImageUrl = p.MainImageUrl,
                    CategorySlug = p.Category.Slug
                })
                .ToListAsync();

            return new ProductsPageViewModel
            {
                Title = "All Products",
                Products = products
            };
        }

        public async Task<ProductsPageViewModel> GetProductsByCategoryAsync(string categorySlug)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == categorySlug);

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == category.Id && p.IsPublished)
                .OrderBy(p => p.Name)
                .Select(p => new ProductListViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Price = p.Price,
                    MainImageUrl = p.MainImageUrl,
                    CategorySlug = p.Category.Slug
                })
                .ToListAsync();

            return new ProductsPageViewModel
            {
                Title = category.Name,
                Products = products
            };
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsAsync(string categorySlug, string productSlug)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => 
	                p.Slug == productSlug && 
	                p.Category.Slug == categorySlug && 
	                p.IsPublished);

            if (product == null)
	            throw new ArgumentException("Product Not Found");

            var productVariant = await _context.ProductVariants
                .AsNoTracking()
                .Where(pv => pv.ProductId == product.Id)
                .Select(pv => new VariantDisplayViewModel()
                {
                    Id = pv.Id,
                    VariantName = pv.VariantName,
                    Price = pv.Price ?? product.Price
                })
                .ToArrayAsync();

            return new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ProductSlug = product.Slug,
                CategorySlug = product.Category.Slug,
                Description = product.Description,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                ProductVariants = productVariant
            };
        }

        public async Task<ProductFormViewModel> GetProductCreateViewModelAsync()
        {
            IEnumerable<CategoryViewModel> categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToArrayAsync();

            if (!categories.Any())
                throw new ArgumentException("Categories Not Found");

            ProductFormViewModel model = new ProductFormViewModel
            {
                Categories = categories
            };

            return model;
        }

        public async Task<string> AddProductAsync(ProductFormViewModel model)
        {
            var slug = GenerateSlug(model.Name);

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                MainImageUrl = model.MainImageUrl,
                CategoryId = model.CategoryId,
                Slug = slug,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                IsPublished = true,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product.Slug;
        }

        public async Task<ManageVariantsViewModel> GetProductAsync(string productSlug)
        {
            var model = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductVariants)
                .Include(p => p.Category)
                .Where(p => p.Slug == productSlug && p.IsPublished)
                .Select(p => new ManageVariantsViewModel()
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductSlug = p.Slug,
                    CategorySlug = p.Category.Slug,
                    Variants = p.ProductVariants.Select(pv => new VariantEditViewModel
                        {
                            Id = pv.Id,
                            Name = pv.VariantName,
                            Price = pv.Price
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (model == null)
	            throw new ArgumentException("Product Not Found");

            return model;
        }

        public async Task ManageProductVariantsAsync(ManageVariantsViewModel model)
        {
            var productVariantExistingInDatabase = await _context.ProductVariants
                .Where(pv => pv.ProductId == model.ProductId)
                .ToListAsync();

            var incomingIdList = model.Variants.Select(v => v.Id).ToList();
            var toDeleteList = productVariantExistingInDatabase
	            .Where(pv => !incomingIdList.Contains(pv.Id)).ToList();

            _context.ProductVariants.RemoveRange(toDeleteList);

            foreach (var variant in model.Variants)
            {
                if (variant.Id > 0)
                {
                    var existing = productVariantExistingInDatabase!.FirstOrDefault(ev => ev.Id == variant.Id);

                    if (existing != null)
                    {
                        existing.VariantName = variant.Name;
                        existing.Price = variant.Price;
                    }
                }
                else
                {
                    var newVariant = new ProductVariant
                    {
                        ProductId = model.ProductId,
                        VariantName = variant.Name,
                        Price = variant.Price
                    };
                    await _context.AddAsync(newVariant);
                }
            }

            await _context.SaveChangesAsync();
		}

        public async Task<ProductFormViewModel> GetProductEditViewModelAsync(string productSlug)
        {
	        var product = await _context.Products
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Slug == productSlug && p.IsPublished);

            if (product == null)
				throw new ArgumentException("Product Not Found");

            return new ProductFormViewModel
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				MainImageUrl = product.MainImageUrl,
				CategoryId = product.CategoryId,
				Categories = await _context.Categories
					.AsNoTracking()
					.OrderBy(c => c.Name)
					.Select(c => new CategoryViewModel
					{
						Id = c.Id,
						Name = c.Name,
					})
					.ToArrayAsync()
			};
		}

        public async Task<ProductFormViewModel> GetProductEditViewModelAsync(int productId)
        {
			var product = await _context.Products
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == productId && p.IsPublished);

			if (product == null)
				throw new ArgumentException("Product Not Found");

			return new ProductFormViewModel
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				MainImageUrl = product.MainImageUrl,
				CategoryId = product.CategoryId,
				Categories = await _context.Categories
					.AsNoTracking()
					.OrderBy(c => c.Name)
					.Select(c => new CategoryViewModel
					{
						Id = c.Id,
						Name = c.Name,
					})
					.ToArrayAsync()
			};
		}

        public async Task<(string categorySlug, string productSlug)> EditProductAsync(ProductFormViewModel model)
        {
	        var product = await _context.Products
				.FirstOrDefaultAsync(p => p.Id == model.Id && p.IsPublished);

			if (product == null)
				throw new ArgumentException("Product Not Found");

            var categorySlug = await _context.Categories
				.Where(c => c.Id == model.CategoryId)
				.Select(c => c.Slug)
				.FirstOrDefaultAsync();

			product.Name = model.Name;
			product.Description = model.Description;
			product.Price = model.Price;
			product.MainImageUrl = model.MainImageUrl;
			product.CategoryId = model.CategoryId;
			product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
			product.Slug = GenerateSlug(model.Name);

			await _context.SaveChangesAsync();

			return (categorySlug!, product.Slug);
		}

        public async Task DeleteProductAsync(string productSlug)
        {
			var product = await _context.Products
				.FirstOrDefaultAsync(p => p.Slug == productSlug && p.IsPublished);

            if (product == null)
                throw new ArgumentException("Product Not Found");

            var cartItemsForRemoving = await _context.CartItems
                .Where(ci => ci.Product.Slug == productSlug)
                .ToListAsync();

            if (cartItemsForRemoving.Any())
            {
                _context.CartItems.RemoveRange(cartItemsForRemoving);
            }

            product.IsPublished = false;
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            product.Slug = $"{product.Slug}-{DateTime.UtcNow:yyyyMMdd-HHmm}";

	        await _context.SaveChangesAsync();
		}

        private static string GenerateSlug(string name)
        {
            name = name.ToLowerInvariant();
            name = Regex.Replace(name, @"[^a-z0-9\s]", "");
            name = Regex.Replace(name, @"\s+", "-");
            name = name.Trim('-');
            return name;
        }
    }
}
