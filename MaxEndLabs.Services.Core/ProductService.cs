using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;
using MaxEndLabs.Services.Core.Contracts;

using System.Text.RegularExpressions;
using Category = MaxEndLabs.Data.Models.Category;
using Product = MaxEndLabs.Data.Models.Product;
using ProductVariant = MaxEndLabs.Data.Models.ProductVariant;

namespace MaxEndLabs.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(
            IProductRepository productRepository,
            IShoppingCartRepository shoppingCartRepository,
            ICategoryRepository categoryRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> ProductExistsAsync(string productName, int productId)
        {
            var productSlug = GenerateSlug(productName);

            return await _productRepository.SlugExistsAsync(productSlug, productId);
        }

        public async Task<bool> ProductExistsAsync(string productName)
        {
            var productSlug = GenerateSlug(productName);

            return await _productRepository.SlugExistsAsync(productSlug);
        }

        public async Task<ProductPaginationDto> GetProductSearchAsync(string searchTerm, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var products = await _productRepository.GetSearchProductsAsync(searchTerm, skip, pageSize);
            var count = await _productRepository.GetCountAsync(searchTerm);

            if (products == null)
                throw new EntityNotFoundException();

            bool hasPreviousPage = page > 1;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new ProductPaginationDto
            {
                CurrentPage = page,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = page < totalPages,
                Products = products!.Select(p => new ProductPaginationEntityDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Slug = p.Slug,
                    MainImageUrl = p.MainImageUrl,
                    IsPublished = p.IsPublished,
                    CategoryName = p.Category.Name,
                    CategorySlug = p.Category.Slug
                }).ToList()
            };
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository
                    .GetAllCategoriesAsync();

            if (categories == null)
                throw new EntityNotFoundException();

            var categoryDtoList = categories
                .OrderByDescending(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug
                })
                .ToList();

            return categoryDtoList;
        }

        public async Task<ProductsPageDto> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            if (products == null)
                throw new EntityNotFoundException();

            var productDtoList = products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                MainImageUrl = p.MainImageUrl,
                CategorySlug = p.Category.Slug
            })
            .ToList();

            return new ProductsPageDto
            {
                Title = "All Products",
                Products = productDtoList
            };
        }

        public async Task<ProductsPageDto> GetProductsByCategoryAsync(string categorySlug)
        {
            Category? category = await _categoryRepository.GetCategoryAsync(categorySlug);

            if (category == null)
                throw new EntityNotFoundException();

            var products = await _productRepository.GetProductsByCategoryIdAsync(category.Id);

            if (products == null)
                throw new EntityNotFoundException();


            var productsListDto = products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                MainImageUrl = p.MainImageUrl,
                CategorySlug = p.Category.Slug
            })
            .ToList();

            return new ProductsPageDto
            {
                Title = category.Name,
                Products = productsListDto
            };
        }

        public async Task<ProductDetailsDto> GetProductDetailsAsync(string productSlug, bool isFiltered)
        {
            var product = await _productRepository
                .GetProductAsync(productSlug, isFiltered: isFiltered, includeCategory: true, includeVariants: true);

            if (product == null)
                throw new EntityNotFoundException();

            var productVariant = product.ProductVariants
                .Where(pv => !pv.IsDeleted)
                .Select(pv => new ProductVariantDto()
                {
                    Id = pv.Id,
                    VariantName = pv.VariantName,
                    Price = pv.Price ?? product.Price
                })
                .ToArray();

            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                CategorySlug = product.Category.Slug,
                Description = product.Description,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                IsPublished = product.IsPublished,
                ProductVariants = productVariant
            };
        }

        public async Task<ProductFormDto> GetProductCreateDtoAsync()
        {
            var categories = await _categoryRepository
                    .GetAllCategoriesAsync();

            if (categories == null)
                throw new EntityNotFoundException();

            var categoryDtoList = categories
                .OrderBy(c => c.Name)
                .Select(c => new CategorySelectDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();

            var model = new ProductFormDto
            {
                Categories = categoryDtoList
            };

            return model;
        }

        public async Task<string> AddProductAsync(ProductCreateDto dto)
        {
            var slug = GenerateSlug(dto.Name);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                MainImageUrl = dto.MainImageUrl,
                CategoryId = dto.CategoryId,
                Slug = slug,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                IsPublished = true,
            };

            await _productRepository.AddProductAsync(product);

            await EnsureSaveChangesAsync();

            return product.Slug;
        }

        public async Task<ProductVariantListDto> GetProductAsync(string productSlug, bool isFiltered)
        {
            var product = await _productRepository
                .GetProductAsync(productSlug, isFiltered: isFiltered, includeCategory: true, includeVariants: true);

            if (product == null)
                throw new EntityNotFoundException();

            var productDto = new ProductVariantListDto()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                CategorySlug = product.Category.Slug,
                Variants = product.ProductVariants
                    .Where(pv => !pv.IsDeleted)
                    .Select(pv => new ProductVariantDto()
                    {
                        Id = pv.Id,
                        VariantName = pv.VariantName,
                        Price = pv.Price
                    })
                    .ToList()
            };

            return productDto;
        }

        public async Task ManageProductVariantsAsync(ProductVariantListDto dto)
        {
            var productVariantExistingInDatabase =
                await _productRepository.GetProductVariantsByProductIdAsync(dto.ProductId);

            if (productVariantExistingInDatabase == null)
                throw new EntityNotFoundException();

            var incomingIdList = dto.Variants.Select(v => v.Id).ToList();

            var pvToDeleteList = productVariantExistingInDatabase
                .Where(pv => !incomingIdList.Contains(pv.Id)).ToList();

            bool changesMade = false;
            foreach (var productVariant in pvToDeleteList)
            {
                if (productVariant.IsDeleted == false)
                {
                    productVariant.IsDeleted = true;
                    changesMade = true;
                }
            }

            if (changesMade)
                _productRepository.UpdateRangeProductVariantAsync(pvToDeleteList);

            foreach (var variant in dto.Variants)
            {
                if (variant.Id > 0 ||
                    productVariantExistingInDatabase.Any(ev => ev.VariantName == variant.VariantName))
                {
                    var existing = productVariantExistingInDatabase!
                        .FirstOrDefault(ev => (ev.Id == variant.Id) || (ev.VariantName == variant.VariantName));

                    if (existing != null)
                    {
                        existing.VariantName = variant.VariantName;
                        existing.Price = variant.Price;
                        existing.IsDeleted = false;
                    }
                }
                else
                {
                    var newVariant = new ProductVariant()
                    {
                        ProductId = dto.ProductId,
                        VariantName = variant.VariantName,
                        Price = variant.Price
                    };

                    await _productRepository.AddProductVariantAsync(newVariant);
                }
            }

            await EnsureSaveChangesAsync();
        }

        public async Task<ProductFormDto> GetProductEditDtoAsync(string productSlug)
        {
            var product = await _productRepository
                .GetProductAsync(productSlug, isFiltered: true, includeCategory: false, includeVariants: false);
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            if (product == null || categories == null)
                throw new EntityNotFoundException();

            return new ProductFormDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                CategoryId = product.CategoryId,
                Categories = categories
                    .OrderBy(c => c.Name)
                    .Select(c => new CategorySelectDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                    .ToList()
            };
        }

        public async Task<(string categorySlug, string productSlug)> EditProductAsync(ProductFormDto dto)
        {
            var category = await _categoryRepository.GetCategoryAsync(dto.CategoryId);
            if (category == null)
                throw new EntityNotFoundException();

            var product = await _productRepository.GetProductAsync(dto.Id);

            if (product == null)
                throw new EntityNotFoundException();

            var categorySlug = product.Category.Slug;

            if (categorySlug == null)
                throw new EntityNotFoundException();

            bool changesMade = false;
            if (product.Name != dto.Name || product.Description != dto.Description ||
                product.Price != dto.Price || product.MainImageUrl != dto.MainImageUrl ||
                product.CategoryId != dto.CategoryId)
            {
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.MainImageUrl = dto.MainImageUrl;
                product.CategoryId = dto.CategoryId;
                
                changesMade = true;
            }

            if (changesMade)
            {
                product.Slug = GenerateSlug(dto.Name);
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _productRepository.ProductUpdate(product);
            }

            await EnsureSaveChangesAsync();

            return (category.Slug, product.Slug);
        }

        public async Task SoftDeleteProductAsync(string productSlug)
        {
            var product = await _productRepository
                .GetProductAsync(productSlug, isFiltered: true, includeCategory: false, includeVariants: true);

            if (product == null)
                throw new EntityNotFoundException();

            var cartItemsForRemoving = await _shoppingCartRepository
                .GetCartItemsByProductSlugAsync(product.Slug);

            if (cartItemsForRemoving == null)
                throw new EntityNotFoundException();

            if (cartItemsForRemoving.Any() && product.IsPublished)
                _shoppingCartRepository.CartItemsRemoveRange(cartItemsForRemoving);

            if (product.IsPublished)
            {
                product.IsPublished = false;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                product.Slug = $"{product.Slug}-{DateTime.UtcNow:yyyyMMdd-HHmmss}";

                foreach (var productVariant in product.ProductVariants)
                {
                    productVariant.IsDeleted = true;
                }

                _productRepository.ProductUpdate(product);
            }

            await EnsureSaveChangesAsync();
        }

        public async Task RestoreProductAsync(string productSlug)
        {
            var product = await _productRepository
                .GetProductAsync(productSlug, isFiltered: false, includeCategory: false, includeVariants: true);

            if (product == null)
                throw new EntityNotFoundException();

            if (product.IsPublished == false)
            {
                string pattern = @"-\d{8}-\d{6}$";

                product.IsPublished = true;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                product.Slug = Regex.Replace(product.Slug, pattern, string.Empty);

                foreach (var productVariant in product.ProductVariants)
                {
                    productVariant.IsDeleted = false;
                }

                _productRepository.RestoreProduct(product);
            }
            
            await EnsureSaveChangesAsync();
        }

        private static string GenerateSlug(string name)
        {
            name = name.ToLowerInvariant();
            name = Regex.Replace(name, @"[^a-z0-9\s]", "");
            name = Regex.Replace(name, @"\s+", "-");
            name = name.Trim('-');
            return name;
        }

        private async Task EnsureSaveChangesAsync()
        {
            int changes = await _productRepository.SaveChangesAsync();

            var successAdd = changes > 0;

            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}