using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            if (category == null)
                return null;

            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == category.Id)
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
                .FirstOrDefaultAsync(p => p.Slug == productSlug && p.Category.Slug == categorySlug);

            if (product == null)
                return null;

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

        public async Task<ProductCreateViewModel> GetProductCreateViewModelAsync()
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

            ProductCreateViewModel model = new ProductCreateViewModel
            {
                Categories = categories
            };

            return model;
        }

        public async Task<int> CreateProductAsync(ProductCreateViewModel model)
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

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<ManageVariantsViewModel> GetProductByIdAsync(int productId)
        {
            var model = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductVariants)
                .Include(p => p.Category)
                .Where(p => p.Id == productId)
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

            return model!;
        }

        public async Task ManageProductVariantsAsync(ManageVariantsViewModel model)
        {
            var existingVariants = await _context.ProductVariants
                .Where(pv => pv.ProductId == model.ProductId)
                .ToListAsync();

            foreach (var variant in model.Variants)
            {
                if (variant.Id > 0)
                {
                    var existing = existingVariants!.FirstOrDefault(ev => ev.Id == variant.Id);

                    if (existingVariants is not null)
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

                await _context.SaveChangesAsync();
            }
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
