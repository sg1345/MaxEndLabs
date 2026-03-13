using System.Text.RegularExpressions;

using MaxEndLabs.Data.Models;

using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;
using MaxEndLabs.Services.Core.Contracts;

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

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository
		            .GetAllCategoriesAsync();

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
            var category = await _categoryRepository.GetCategoryBySlugAsync(categorySlug);

            if (category == null)
                throw new ArgumentException("Category Not Found");

            var products = await _productRepository.GetProductsByCategoryIdAsync(category.Id);

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

        public async Task<ProductDetailsDto> GetProductDetailsAsync(string productSlug)
        {
            var product = await _productRepository.GetProductAsync(productSlug);

            if (product == null)
	            throw new ArgumentException("Product Not Found");

            var productVariant = product.ProductVariants
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
                ProductVariants = productVariant
            };
        }

        public async Task<ProductFormDto> GetProductCreateDtoAsync()
        {
            var categories = await _categoryRepository
		            .GetAllCategoriesAsync();

            if (!categories.Any())
	            throw new ArgumentException("Categories Not Found");

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

        public async Task<ProductVariantListDto> GetProductAsync(string productSlug)
        {
            var product = await _productRepository.GetProductAsync(productSlug);

            if (product == null)
	            throw new ArgumentException("Product Not Found");

			var productDto = new ProductVariantListDto()
            {
	            ProductId = product.Id,
	            ProductName = product.Name,
	            ProductSlug = product.Slug,
	            CategorySlug = product.Category.Slug,
	            Variants = product.ProductVariants.Select(pv => new ProductVariantDto()
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

			var incomingIdList = dto.Variants.Select(v => v.Id).ToList();
			
			var pvToDeleteList = productVariantExistingInDatabase
	            .Where(pv => !incomingIdList.Contains(pv.Id)).ToList();

            _productRepository.RemoveRangeProductVariantAsync(pvToDeleteList);

            foreach (var variant in dto.Variants)
            {
                if (variant.Id > 0)
                {
                    var existing = productVariantExistingInDatabase!.FirstOrDefault(ev => ev.Id == variant.Id);

                    if (existing != null)
                    {
                        existing.VariantName = variant.VariantName;
                        existing.Price = variant.Price;
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
	        var product = await _productRepository.GetProductAsync(productSlug);
            var categories = await _categoryRepository.GetAllCategoriesAsync();

			if (product == null)
				throw new ArgumentException("Product Not Found");

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

        public async Task<ProductFormDto> GetProductEditDtoAsync(int productId)
        {
			var product = await _productRepository.GetProductAsync(productId);
			var categories = await _categoryRepository.GetAllCategoriesAsync();

			if (product == null)
				throw new ArgumentException("Product Not Found");

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
	        var product = await _productRepository.GetProductAsync(dto.Id);

			if (product == null)
				throw new ArgumentException("Product Not Found");

			var categorySlug = await _categoryRepository.GetCategorySlugAsync(dto.CategoryId);

			if (categorySlug == null)
				throw new ArgumentException("Category Not Found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.MainImageUrl = dto.MainImageUrl;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            product.Slug = GenerateSlug(dto.Name);

            await EnsureSaveChangesAsync();

			return (categorySlug, product.Slug);
		}

        public async Task DeleteProductAsync(string productSlug)
        {
	        var product = await _productRepository.GetProductAsync(productSlug);

            if (product == null)
                throw new ArgumentException("Product Not Found");

            var cartItemsForRemoving = await _shoppingCartRepository.GetCartItemsByProductSlugAsync(product.Slug);

            if (cartItemsForRemoving.Any())
            {
                _shoppingCartRepository.CartItemsRemoveRange(cartItemsForRemoving);
            }

            _productRepository.SoftDeleteProduct(product);

            int changes = await _productRepository.SaveChangesAsync();

            var successAdd = changes > 0;

            if (!successAdd)
            {
	            throw new ArgumentException();
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

        private async Task EnsureSaveChangesAsync()
        {
	        int changes = await _productRepository.SaveChangesAsync();

	        var successAdd = changes > 0;

	        if (!successAdd)
	        {
		        throw new ArgumentException("Database Operation Failed");
	        }
		}
    }
}
