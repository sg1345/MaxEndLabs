using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.Category;
using MaxEndLabs.Service.Models.Product;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using Moq;
using Product = MaxEndLabs.Data.Models.Product;
using ProductService = MaxEndLabs.Services.Core.ProductService;

namespace MaxEndLabs.Service.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService productService;

        private Mock<IShoppingCartRepository> shoppingCartRepositoryMock;
        private Mock<IProductRepository> productRepositoryMock;
        private Mock<ICategoryRepository> categoryRepositoryMock;

        private Guid _productId;
        private Guid _variantId1;
        private Guid _variantId2;
        private Guid _variantId3;
        private Guid _categoryId1;
        private Guid _categoryId2;
        private Guid _categoryId3;
        private Guid _cartItemId1;
        private Guid _cartItemId2;

        [SetUp]
        public void Setup()
        {
            shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
            productRepositoryMock = new Mock<IProductRepository>();
            categoryRepositoryMock = new Mock<ICategoryRepository>();

            productService = new ProductService(
                productRepositoryMock.Object,
                shoppingCartRepositoryMock.Object,
                categoryRepositoryMock.Object);

            _productId = Guid.Parse("057e6259-55b4-4ddd-9d0f-c1b11cb1f2f0");
            _variantId1 = Guid.Parse("1d3c10bc-ee5f-4423-8130-e9cd31603392");
            _variantId2 = Guid.Parse("a24671ae-0d78-430e-b5f5-f4b01de473a1");
            _variantId3 = Guid.Parse("76428bde-2653-4a65-a896-80f4cd26c592");
            _categoryId1 = Guid.Parse("01c3ab5c-7f7d-4340-b28e-39fc72156472");
            _categoryId2 = Guid.Parse("11fb7c7b-571e-40d3-b81f-56b4e4a53e1f");
            _categoryId3 = Guid.Parse("5ee35de2-a2b7-4adc-9a3b-00199b427c1c");
            _cartItemId1 = Guid.Parse("01c3ab5c-7f7d-4340-b28e-39fc72156472");
            _cartItemId2 = Guid.Parse("00d38a13-7f09-4df4-a924-de41bc22ea5d");
        }

        [Test]
        public async Task ProductExistsAsync_ProductSearchedBySlugAndId_ReturnTrue()
        {
            //Arrange
            string productName = "Test Shirt";
            Guid productId = _productId;
            string productSlug = "test-shirt";

            productRepositoryMock.Setup(pr => pr.SlugExistsAsync(productSlug, productId))
                .ReturnsAsync(true);
            //Act
            var result = await productService.ProductExistsAsync(productName, productId);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ProductExistsAsync_ProductSearchedBySlugAndId_ReturnFalse()
        {
            //Arrange
            string productName = "Test Shirt";
            var productId = _productId;
            string productSlug = "test-shirt";

            productRepositoryMock.Setup(pr => pr.SlugExistsAsync(productSlug, productId))
                .ReturnsAsync(false);
            //Act
            var result = await productService.ProductExistsAsync(productName, productId);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ProductExistsAsync_ProductSearchedBySlug_ReturnTrue()
        {
            //Arrange
            string productName = "Test Shirt";
            string productSlug = "test-shirt";

            productRepositoryMock.Setup(pr => pr.SlugExistsAsync(productSlug))
                .ReturnsAsync(true);
            //Act
            var result = await productService.ProductExistsAsync(productName);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ProductExistsAsync_ProductSearchedBySlug_ReturnFalse()
        {
            //Arrange
            string productName = "Test Shirt";
            string productSlug = "test-shirt";

            productRepositoryMock.Setup(pr => pr.SlugExistsAsync(productSlug))
                .ReturnsAsync(false);
            //Act
            var result = await productService.ProductExistsAsync(productName);

            //Assert
            Assert.That(result, Is.False);
        }


        [Test]
        [TestCase(null)]
        [TestCase("not null")]
        public async Task GetProductSearchAsync_Searched_ReturnsCorrectPopulatedDto(string? mainImageUrl)
        {
            //Arrange
            string searchTerm = "Something";
            int page = 1;
            int pageSize = 2;


            var product = new Product
            {
                Id = _productId,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Category = new Category { Name = "Electronics", Slug = "electronics" },
                Price = 89.99m,
                MainImageUrl = mainImageUrl,
                IsPublished = true
            };

            productRepositoryMock
                .Setup(pr => pr.GetSearchProductsAsync(searchTerm, 0, 2))
                .ReturnsAsync(new List<Product>()
                {
                    product
                });
            productRepositoryMock
                .Setup(pr => pr.GetCountAsync(searchTerm))
                .ReturnsAsync(1);


            //act
            var result = await productService.GetProductSearchAsync(searchTerm, page, pageSize);

            var resultProductsList = result.Products.ToList();

            var expected = new ProductPaginationDto
            {
                CurrentPage = page,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false,
                Products = new List<ProductPaginationEntityDto>
                {
                    new ProductPaginationEntityDto
                    {
                        Id = _productId,
                        Name = product.Name,
                        Price = product.Price,
                        Slug = product.Slug,
                        MainImageUrl = product.MainImageUrl,
                        IsPublished = product.IsPublished,
                        CategoryName = product.Category.Name,
                        CategorySlug = product.Category.Slug
                    }
                }
            };

            var expectedProductList = expected.Products.ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.CurrentPage, Is.EqualTo(expected.CurrentPage));
                Assert.That(result.TotalPages, Is.EqualTo(expected.TotalPages));
                Assert.That(result.HasPreviousPage, Is.EqualTo(expected.HasPreviousPage));
                Assert.That(result.HasNextPage, Is.EqualTo(expected.HasNextPage));
                Assert.That(result.Products.Count(), Is.EqualTo(expected.Products.Count()));
                Assert.That(resultProductsList[0].Id, Is.EqualTo(expectedProductList[0].Id));
                Assert.That(resultProductsList[0].Name, Is.EqualTo(expectedProductList[0].Name));
                Assert.That(resultProductsList[0].Price, Is.EqualTo(expectedProductList[0].Price));
                Assert.That(resultProductsList[0].Slug, Is.EqualTo(expectedProductList[0].Slug));
                Assert.That(resultProductsList[0].MainImageUrl, Is.EqualTo(expectedProductList[0].MainImageUrl));
                Assert.That(resultProductsList[0].IsPublished, Is.EqualTo(expectedProductList[0].IsPublished));
                Assert.That(resultProductsList[0].CategoryName, Is.EqualTo(expectedProductList[0].CategoryName));
                Assert.That(resultProductsList[0].CategorySlug, Is.EqualTo(expectedProductList[0].CategorySlug));
            });
        }

        [Test]
        public async Task GetProductSearchAsync_SearchedWithWrongTerm_ReturnsCorrectEmptyProducts()
        {
            //Arrange
            string searchTerm = "Something";
            int page = 1;
            int pageSize = 2;

            productRepositoryMock
                .Setup(or => or.GetSearchProductsAsync(searchTerm, 0, 2))
                .ReturnsAsync(new List<Product>());
            productRepositoryMock
                .Setup(pr => pr.GetCountAsync(searchTerm))
                .ReturnsAsync(0);

            //Act
            var result = await productService.GetProductSearchAsync(searchTerm, page, pageSize);

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(0));
        }

        [Test]
        [TestCase(1, 10, 0)]
        [TestCase(2, 10, 10)]
        [TestCase(3, 5, 10)]
        public async Task GetProductSearchAsync_VariousPages_CallsRepoWithCorrectSkip(int page, int pageSize, int expectedSkip)
        {
            // Arrange
            string searchTerm = "Something";
            string searchType = "someType";

            productRepositoryMock.Setup(r => r.GetSearchProductsAsync(searchTerm, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Product>());

            productRepositoryMock.Setup(r => r.GetCountAsync(searchTerm))
                .ReturnsAsync(100);

            // Act
            var result = await productService.GetProductSearchAsync(searchTerm, page, pageSize);

            // Assert
            productRepositoryMock.Verify(r => r.GetSearchProductsAsync(
                    searchTerm,
                    expectedSkip,
                    pageSize),
                Times.Once);
        }

        [Test]
        [TestCase(11, 5, 3)]
        [TestCase(10, 5, 2)]
        [TestCase(0, 5, 0)]
        public async Task GetProductSearchAsync_MultipleScenarios_CalculatesTotalPagesCorrectly
            (int totalCount, int pageSize, int expectedPages)
        {
            // Arrange
            string searchTerm = "Something";
            productRepositoryMock.Setup(r => r.GetSearchProductsAsync(searchTerm, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Product>());
            productRepositoryMock.Setup(r => r.GetCountAsync(searchTerm))
                .ReturnsAsync(totalCount);

            // Act
            var result = await productService.GetProductSearchAsync(searchTerm, 1, pageSize);

            // Assert
            Assert.That(result.TotalPages, Is.EqualTo(expectedPages));
        }

        [Test]
        public void GetProductSearchAsync_ProductsNotFound_ThrowsEntityNotFoundException()
        {
            //Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            int page = 2;
            int pageSize = 2;

            productRepositoryMock.Setup(r => r.GetSearchProductsAsync(searchTerm, It.IsAny<int>(), pageSize))!
                .ReturnsAsync((List<Product>?)null);
            productRepositoryMock.Setup(r => r.GetCountAsync(searchTerm))
                .ReturnsAsync(It.IsAny<int>());

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductSearchAsync(searchTerm, page, pageSize));
        }

        [Test]
        [TestCase(1, 5, 5, false, false)]
        [TestCase(1, 5, 6, false, true)]
        [TestCase(2, 5, 6, true, false)]
        [TestCase(2, 5, 11, true, true)]
        public async Task GetProductSearchAsync_MultipleScenarios_CheckLogicHasPreviousAndNextPage
            (int page, int pageSize, int totalCount, bool hasPrevious, bool hasNext)
        {
            //Arrange
            string searchTerm = "Something";
            productRepositoryMock.Setup(r => r.GetSearchProductsAsync(searchTerm, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Product>());
            productRepositoryMock.Setup(r => r.GetCountAsync(searchTerm))
                .ReturnsAsync(totalCount);

            //Act
            var result = await productService.GetProductSearchAsync(searchTerm, page, pageSize);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.HasPreviousPage, Is.EqualTo(hasPrevious));
                Assert.That(result.HasNextPage, Is.EqualTo(hasNext));
            });
        }

        [Test]
        public async Task GetAllCategoriesAsync_CategoriesExist_ReturnCorrectCategories()
        {
            //Arrange
            var category = new Category { Id = _categoryId1, Name = "a category", Slug = "a-category" };

            var expected = new CategoryDto { Id = _categoryId1, Name = "a category", Slug = "a-category" };

            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Category> { category });

            //Act
            var result = await productService.GetAllCategoriesAsync();

            //Assert
            var resultList = result.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(resultList.Count, Is.EqualTo(1));
                Assert.That(resultList[0].Id, Is.EqualTo(expected.Id));
                Assert.That(resultList[0].Name, Is.EqualTo(expected.Name));
                Assert.That(resultList[0].Slug, Is.EqualTo(expected.Slug));
            });
        }

        [Test]
        public async Task GetAllCategoriesAsync_CategoriesAreEmpty_ReturnCorrectEmptyList()
        {
            //Arrange
            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Category>());

            //Act
            var result = await productService.GetAllCategoriesAsync();

            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAllCategoriesAsync_CategoryListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync((List<Category>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetAllCategoriesAsync());
        }

        [Test]
        public void GetAllProductsAsync_ProductListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync((List<Product>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetAllProductsAsync());
        }

        [Test]
        public async Task GetAllProductsAsync_ProductListEmpty_ReturnCorrectEmptyList()
        {
            //Arrange
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());

            //Act
            var result = await productService.GetAllProductsAsync();

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(0));
        }

        [Test]
        [TestCase(null)]
        [TestCase("not null")]
        public async Task GetAllProductsAsync_ProductList_ReturnCorrectPopulatedDto(string? mainImageUrl)
        {
            //Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = _productId,
                    Name = "Ultra-Light Wireless Mouse",
                    Slug = "ultra-light-wireless-mouse",
                    Category = new Category { Slug = "electronics" },
                    Price = 89.99m,
                    MainImageUrl = mainImageUrl,
                }
            };
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync(products);

            //Act
            var result = await productService.GetAllProductsAsync();

            //Assert
            var productsResult = result.Products.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo("All Products"));
                Assert.That(productsResult.Count(), Is.EqualTo(1));
                Assert.That(productsResult[0].Id, Is.EqualTo(products[0].Id));
                Assert.That(productsResult[0].Name, Is.EqualTo(products[0].Name));
                Assert.That(productsResult[0].Slug, Is.EqualTo(products[0].Slug));
                Assert.That(productsResult[0].Price, Is.EqualTo(products[0].Price));
                Assert.That(productsResult[0].MainImageUrl, Is.EqualTo(products[0].MainImageUrl));
                Assert.That(productsResult[0].CategorySlug, Is.EqualTo(products[0].Category.Slug));
            });

        }

        [Test]
        [TestCase(null)]
        [TestCase("not null")]
        public async Task GetProductsByCategoryAsync_ProductList_ReturnCorrectPopulatedDto(string? mainImageUrl)
        {
            //Arrange
            string productSlug = "electronics";
            string categoryName = "Electronics";
            var products = new List<Product>
            {
                new Product
                {
                    Id = _productId,
                    Name = "Ultra-Light Wireless Mouse",
                    Slug = "ultra-light-wireless-mouse",
                    Category = new Category { Id = _categoryId1, Slug = "electronics" },
                    Price = 89.99m,
                    MainImageUrl = mainImageUrl,
                }
            };
            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(productSlug))
                .ReturnsAsync(new Category
                {
                    Id = products[0].Category.Id,
                    Name = categoryName
                });

            productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(products[0].Category.Id))
                .ReturnsAsync(products);

            //Act
            var result = await productService.GetProductsByCategoryAsync(productSlug);

            //Assert
            var productsResult = result.Products.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo(categoryName));
                Assert.That(productsResult.Count(), Is.EqualTo(1));
                Assert.That(productsResult[0].Id, Is.EqualTo(products[0].Id));
                Assert.That(productsResult[0].Name, Is.EqualTo(products[0].Name));
                Assert.That(productsResult[0].Slug, Is.EqualTo(products[0].Slug));
                Assert.That(productsResult[0].Price, Is.EqualTo(products[0].Price));
                Assert.That(productsResult[0].MainImageUrl, Is.EqualTo(products[0].MainImageUrl));
                Assert.That(productsResult[0].CategorySlug, Is.EqualTo(products[0].Category.Slug));
            });
        }

        [Test]
        public async Task GetProductsByCategoryAsync_ProductListEmpty_ReturnCorrectEmptyList()
        {
            //Arrange
            string categorySlug = "slug";

            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(categorySlug))
                .ReturnsAsync(new Category
                {
                    Id = _categoryId1,
                });

            productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(_categoryId1))
                .ReturnsAsync(new List<Product>());

            //Act
            var result = await productService.GetAllProductsAsync();

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(0));
        }


        [Test]
        public void GetProductsByCategoryAsync_ProductListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            string categorySlug = "slug";

            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(categorySlug))
                .ReturnsAsync(new Category
                {
                    Id = _categoryId1,
                });
            productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(_categoryId1))
                .ReturnsAsync((List<Product>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.GetProductsByCategoryAsync(categorySlug));
        }

        [Test]
        public void GetProductsByCategoryAsync_CategoryIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            string categorySlug = "slug";

            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(categorySlug))
                .ReturnsAsync((Category?)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.GetProductsByCategoryAsync(categorySlug));
        }

        [Test]
        public void GetProductDetailsAsync_ProductNotFound_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            productRepositoryMock.Setup(pr =>
                    pr.GetProductAsync(productSlug, true, true, true))
                .ReturnsAsync((Product)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.GetProductDetailsAsync(productSlug, true));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("not null", "not null")]
        public async Task GetProductDetailsAsync_ProductExist_ReturnPopulatedDto(string? description, string? mainImageUrl)
        {
            //Arrange
            string productSlug = "ultra-light-wireless-mouse";

            var product = new Product
            {
                Id = _productId,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Description = description,
                Category = new Category
                {
                    Name = "Electronics",
                    Slug = "electronics"
                },
                Price = 89.99m,
                MainImageUrl = mainImageUrl,
                ProductVariants = new List<ProductVariant>()
                {
                    new ProductVariant() { Id = _variantId1, VariantName = "Ultra white", Price = 69.69m, IsDeleted = false },
                    new ProductVariant() { Id = _variantId2, VariantName = "Giga Black", Price = null, IsDeleted = false },
                    new ProductVariant() { Id = _variantId3, VariantName = "Cyber Myber", Price = null, IsDeleted = true }
                }
            };
            var expected = new ProductDetailsDto()
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                CategorySlug = product.Category.Slug,
                Description = product.Description,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                ProductVariants = new List<ProductVariantDto>()
                {
                    new ProductVariantDto() { Id = _variantId1, VariantName = "Ultra white", Price = 69.69m, },
                    new ProductVariantDto() { Id = _variantId2, VariantName = "Giga Black", Price = 89.99m }
                }
            };
            var expectedVariantsArr = expected.ProductVariants.ToArray();

            productRepositoryMock.Setup(pr =>
                    pr.GetProductAsync(productSlug, true, true, true))
                .ReturnsAsync(product);
            //Act
            var result = await productService.GetProductDetailsAsync(productSlug, true);

            //Assert
            var variantResultArr = result.ProductVariants.ToArray();
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Name, Is.EqualTo(expected.Name));
                Assert.That(result.Slug, Is.EqualTo(expected.Slug));
                Assert.That(result.CategorySlug, Is.EqualTo(expected.CategorySlug));
                Assert.That(result.Description, Is.EqualTo(expected.Description));
                Assert.That(result.Price, Is.EqualTo(expected.Price));
                Assert.That(result.MainImageUrl, Is.EqualTo(expected.MainImageUrl));
                Assert.That(result.ProductVariants.Count(), Is.EqualTo(expected.ProductVariants.Count()));
                Assert.That(variantResultArr[0].Id, Is.EqualTo(expectedVariantsArr[0].Id));
                Assert.That(variantResultArr[0].VariantName, Is.EqualTo(expectedVariantsArr[0].VariantName));
                Assert.That(variantResultArr[0].Price, Is.EqualTo(expectedVariantsArr[0].Price));
                Assert.That(variantResultArr[1].Id, Is.EqualTo(expectedVariantsArr[1].Id));
                Assert.That(variantResultArr[1].VariantName, Is.EqualTo(expectedVariantsArr[1].VariantName));
                Assert.That(variantResultArr[1].Price, Is.EqualTo(expectedVariantsArr[1].Price));
            });
        }

        [Test]
        public void GetProductCreateDtoAsync_CategoriesDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync((List<Category>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductCreateDtoAsync());
        }

        [Test]
        public async Task GetProductCreateDtoAsync_CategoriesExist_ReturnPopulatedDto()
        {
            //Arrange
            var categories = new List<Category>()
            {
                new Category() { Id = _categoryId1, Name = "A" },
                new Category() { Id = _categoryId2, Name = "C" },
                new Category() { Id = _categoryId3, Name = "B" }
            };

            var expectedCat = new List<Category>()
            {
                new Category() { Id = _categoryId1, Name = "A" },
                new Category() { Id = _categoryId3, Name = "B" },
                new Category() { Id = _categoryId2, Name = "C" }
            };

            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync(categories);
            //Act
            var result = await productService.GetProductCreateDtoAsync();

            //Assert
            var categoriesResult = result.Categories.ToArray();
            Assert.Multiple(() =>
            {
                Assert.That(categoriesResult.Length, Is.EqualTo(expectedCat.Count));
                Assert.That(categoriesResult[0].Id, Is.EqualTo(expectedCat[0].Id));
                Assert.That(categoriesResult[0].Name, Is.EqualTo(expectedCat[0].Name));
                Assert.That(categoriesResult[1].Id, Is.EqualTo(expectedCat[1].Id));
                Assert.That(categoriesResult[1].Name, Is.EqualTo(expectedCat[1].Name));
                Assert.That(categoriesResult[2].Id, Is.EqualTo(expectedCat[2].Id));
                Assert.That(categoriesResult[2].Name, Is.EqualTo(expectedCat[2].Name));
            });
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("not null", "not null")]
        public async Task AddProductAsync_CreateNewProduct_ReturnProductSlugCorrectly(string? description, string? mainImageUrl)
        {
            //Arrange
            ProductCreateDto dto = new ProductCreateDto()
            {
                CategoryId = _categoryId1,
                Description = description,
                MainImageUrl = description,
                Name = "Product Name",
                Price = 19.99m
            };
            Product expected = new Product()
            {
                Id = _productId,
                CategoryId = dto.CategoryId,
                Description = description,
                MainImageUrl = mainImageUrl,
                Name = dto.Name,
                Price = dto.Price,
                IsPublished = true,
                Slug = "product-name"
            };
            Product capturedProduct = null!;

            productRepositoryMock.Setup(or => or.AddProductAsync(It.IsAny<Product>()))
                .Callback<Product>(p => { p.Id = _productId; capturedProduct = p; })
                .Returns(Task.CompletedTask);

            productRepositoryMock.Setup(pr => pr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await productService.AddProductAsync(dto);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expected.Slug));
                Assert.That(capturedProduct.Id, Is.EqualTo(expected.Id));
                Assert.That(capturedProduct.Name, Is.EqualTo(expected.Name));
                Assert.That(capturedProduct.CategoryId, Is.EqualTo(expected.CategoryId));
                Assert.That(capturedProduct.Description, Is.EqualTo(expected.Description));
                Assert.That(capturedProduct.Price, Is.EqualTo(expected.Price));
                Assert.That(capturedProduct.MainImageUrl, Is.EqualTo(expected.MainImageUrl));
                Assert.That(capturedProduct.IsPublished, Is.EqualTo(expected.IsPublished));
            });
        }

        [Test]
        public async Task GetProductAsync_ProductSlugIsCorrect_ReturnPopulatedDto()
        {
            //Arrange
            string productSlug = "ultra-light-wireless-mouse";

            var product = new Product
            {
                Id = _productId,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Category = new Category { Name = "Electronics", Slug = "electronics" },
                Price = 89.99m,
                ProductVariants = new List<ProductVariant>()
                {
                    new ProductVariant { Id = _variantId1, VariantName = "Ultra white", Price = 69.69m, IsDeleted = false},
                    new ProductVariant { Id = _variantId2, VariantName = "Giga Black", Price = null, IsDeleted = false },
                    new ProductVariant { Id = _variantId3, VariantName = "Cyber Myber", Price = null, IsDeleted = true }
                }
            };

            ProductVariantListDto expected = new ProductVariantListDto()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                CategorySlug = product.Category.Slug,
                Variants = new List<ProductVariantDto>
                {
                    new ProductVariantDto { Id = _variantId1, VariantName = "Ultra white", Price = 69.69m },
                    new ProductVariantDto { Id = _variantId2, VariantName = "Giga Black", Price = null }
                }
            };

            productRepositoryMock.Setup(pr => pr.GetProductAsync(productSlug, true, true, true))
                .ReturnsAsync(product);

            //Act
            var result = await productService.GetProductAsync(productSlug, true);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ProductId, Is.EqualTo(expected.ProductId));
                Assert.That(result.ProductName, Is.EqualTo(expected.ProductName));
                Assert.That(result.ProductSlug, Is.EqualTo(expected.ProductSlug));
                Assert.That(result.CategorySlug, Is.EqualTo(expected.CategorySlug));
                Assert.That(result.Variants.Count, Is.EqualTo(expected.Variants.Count));
                Assert.That(result.Variants[0].Id, Is.EqualTo(expected.Variants[0].Id));
                Assert.That(result.Variants[0].VariantName, Is.EqualTo(expected.Variants[0].VariantName));
                Assert.That(result.Variants[0].Price, Is.EqualTo(expected.Variants[0].Price));
                Assert.That(result.Variants[1].Id, Is.EqualTo(expected.Variants[1].Id));
                Assert.That(result.Variants[1].VariantName, Is.EqualTo(expected.Variants[1].VariantName));
                Assert.That(result.Variants[1].Price, Is.EqualTo(expected.Variants[1].Price));
            });
        }

        [Test]
        public void GetProductAsync_ProductDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "ultra-light-wireless-mouse";
            productRepositoryMock.Setup(pr => pr.GetProductAsync(productSlug, true, true, true))
                .ReturnsAsync((Product)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductAsync(productSlug, true));
        }

        [Test]
        public void ManageProductVariantsAsync_ProductDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            ProductVariantListDto dto = new ProductVariantListDto()
            {
                ProductId = _productId,
            };

            productRepositoryMock.Setup(pr => pr.GetProductVariantsByProductIdAsync(dto.ProductId))
                .ReturnsAsync((IEnumerable<ProductVariant>)null!);
            
            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.ManageProductVariantsAsync(dto));
        }

        [Test]
        public async Task ManageProductVariantsAsync_ShouldHandle_DeleteUpdateAndAdd()
        {
            //Arrange
            var existingVariants = new List<ProductVariant>
            {
                new ProductVariant { Id = _variantId1, ProductId = _productId, VariantName = "Old - To Delete", IsDeleted = false },
                new ProductVariant { Id = _variantId2, ProductId = _productId, VariantName = "Old - To Update", IsDeleted = false, Price = 10m }
            };

            var dto = new ProductVariantListDto
            {
                ProductId = _productId,
                Variants = new List<ProductVariantDto>
                {
                    new ProductVariantDto { Id = _variantId2, VariantName = "Updated Name", Price = 20m },
                    new ProductVariantDto { Id = Guid.Empty, VariantName = "Brand New", Price = 30m }
                }
            };

            productRepositoryMock
                .Setup(r => r.GetProductVariantsByProductIdAsync(_productId))
                .ReturnsAsync(existingVariants);

            IEnumerable<ProductVariant> capturedDeleted = null!;
            productRepositoryMock
                .Setup(r => r.UpdateRangeProductVariantAsync(It.IsAny<IEnumerable<ProductVariant>>()))
                .Callback<IEnumerable<ProductVariant>>(pvl => capturedDeleted = pvl);

            ProductVariant capturedNewVariant = null!;
            productRepositoryMock.Setup(r => r.AddProductVariantAsync(It.IsAny<ProductVariant>()))
                .Callback<ProductVariant>(v => capturedNewVariant = v);

            productRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await productService.ManageProductVariantsAsync(dto);

            //Assert
            Assert.Multiple(() =>
            {
                //Soft Delete
                Assert.That(capturedDeleted, Is.Not.Null);
                var deleted = capturedDeleted.First(v => v.Id == _variantId1);
                Assert.That(deleted.IsDeleted, Is.True);

                // Update
                var capturedDeletedList = capturedDeleted.ToList();
                Assert.That(capturedDeletedList[0].IsDeleted, Is.True);
                Assert.That(existingVariants[1].VariantName, Is.EqualTo("Updated Name"));

                // Add
                Assert.That(capturedNewVariant.VariantName, Is.EqualTo("Brand New"));
                Assert.That(capturedNewVariant.Price, Is.EqualTo(30m));
                productRepositoryMock.Verify(r => r.AddProductVariantAsync(It.IsAny<ProductVariant>()), Times.Once);

                //Save was called
                productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
            });
        }

        [Test]
        public void GetProductEditDtoAsync_ProductDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            productRepositoryMock.Setup(pr => pr.GetProductAsync(productSlug, true, false, false))
                .ReturnsAsync((Product)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductEditDtoAsync(productSlug));
        }

        [Test]
        public void GetProductEditDtoAsync_CategoriesDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            productRepositoryMock.Setup(pr => pr.GetProductAsync(productSlug, true, false, false))
                .ReturnsAsync(new Product());

            categoryRepositoryMock.Setup(pr =>
                    pr.GetAllCategoriesAsync())
                .ReturnsAsync((IEnumerable<Category>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductEditDtoAsync(productSlug));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("not null", "not null")]
        public async Task GetProductEditDtoAsync_ProductAndCategoriesExist_ReturnPopulatedDto
            (string? description, string? mainImageUrl)
        {
            //Arrange
            string productSlug = "ultra-light-wireless-mouse";

            var product = new Product
            {
                Id = _productId,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Price = 89.99m,
                Description = description,
                MainImageUrl = mainImageUrl
            };
            var categories = new List<Category>
            {
                new Category { Id = _categoryId1, Name = "B" },
                new Category { Id = _categoryId2, Name = "C" },
                new Category { Id = _categoryId3, Name = "A" }
            };

            var expected = new ProductFormDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                MainImageUrl = product.MainImageUrl,
                CategoryId = product.CategoryId,
                Categories = new List<CategorySelectDto>
                {
                    new CategorySelectDto{ Id = _categoryId3, Name = "A" },
                    new CategorySelectDto{ Id = _categoryId1, Name = "B" },
                    new CategorySelectDto{ Id = _categoryId2, Name = "C" }
                }
            };

            productRepositoryMock.Setup(pr => pr.GetProductAsync(productSlug, true, false, false))
                .ReturnsAsync(product);
            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            //Act
            var result = await productService.GetProductEditDtoAsync(productSlug);
            
            //Assert
            var resultCategoriesArr = result.Categories.ToArray();
            var expectedCategoriesArr = expected.Categories.ToArray();
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Name, Is.EqualTo(expected.Name));
                Assert.That(result.Description, Is.EqualTo(expected.Description));
                Assert.That(result.Price, Is.EqualTo(expected.Price));
                Assert.That(result.MainImageUrl, Is.EqualTo(expected.MainImageUrl));
                Assert.That(result.CategoryId, Is.EqualTo(expected.CategoryId));
                Assert.That(resultCategoriesArr[0].Id, Is.EqualTo(expectedCategoriesArr[0].Id));
                Assert.That(resultCategoriesArr[0].Name, Is.EqualTo(expectedCategoriesArr[0].Name));
                Assert.That(resultCategoriesArr[1].Id, Is.EqualTo(expectedCategoriesArr[1].Id));
                Assert.That(resultCategoriesArr[1].Name, Is.EqualTo(expectedCategoriesArr[1].Name));
                Assert.That(resultCategoriesArr[2].Id, Is.EqualTo(expectedCategoriesArr[2].Id));
                Assert.That(resultCategoriesArr[2].Name, Is.EqualTo(expectedCategoriesArr[2].Name));
            });
        }

        [Test]
        [TestCase(null, "Some Img", "Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        [TestCase("Bad Mouse", "Some Img", "Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        [TestCase("Good Mouse", null, "Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        [TestCase("Good Mouse", "Other Img", "Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        [TestCase("Good Mouse", "Some Img", "New Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "new-mouse")]
        [TestCase("Good Mouse", "Some Img", "Mouse", 1999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        [TestCase("Good Mouse", "Some Img", "Mouse", 8999, "11fb7c7b-571e-40d3-b81f-56b4e4a53e1f", "B", "mouse")]
        public async Task EditProductAsync_ProductAndCategoriesExist_ReturnPopulatedDto
            (string? description, string? mainImageUrl, string name, int priceInt, string categoryId, string categorySlug, string productSlug)
        {
            //Arrange
            decimal price = priceInt / 100m;

            Guid guidCategoryId = Guid.Parse(categoryId);

            var product = new Product
            {
                Id = _productId,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = _categoryId3,
                Category = new Category { Slug = "A" }
            };

            var receivedDto = new ProductFormDto
            {
                Id = product.Id,
                Name = name,
                Description = description,
                Price = price,
                MainImageUrl = mainImageUrl,
                CategoryId = guidCategoryId,
            };

            var expected = new Product
            {
                Id = receivedDto.Id,
                Name = receivedDto.Name,
                Description = receivedDto.Description,
                Price = receivedDto.Price,
                MainImageUrl = receivedDto.MainImageUrl,
                CategoryId = receivedDto.CategoryId
            };
            string expectedCategorySlug = categorySlug;
            string expectedProductSlug = productSlug;
            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category
                {
                    Id = guidCategoryId,
                    Slug = categorySlug
                });

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync(product);

            Product capturedProduct = null!;
            productRepositoryMock.Setup(pr => pr.ProductUpdate(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p);

            productRepositoryMock.Setup(pr => pr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await productService.EditProductAsync(receivedDto);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(capturedProduct.Name, Is.EqualTo(expected.Name));
                Assert.That(capturedProduct.Description, Is.EqualTo(expected.Description));
                Assert.That(capturedProduct.Price, Is.EqualTo(expected.Price));
                Assert.That(capturedProduct.MainImageUrl, Is.EqualTo(expected.MainImageUrl));
                Assert.That(capturedProduct.CategoryId, Is.EqualTo(expected.CategoryId));
                Assert.That(result.productSlug, Is.EqualTo(expectedProductSlug));
                Assert.That(result.categorySlug, Is.EqualTo(expectedCategorySlug));
            });
        }

        [Test]
        [TestCase("Good Mouse", "Some Img", "Mouse", 8999, "5ee35de2-a2b7-4adc-9a3b-00199b427c1c", "A", "mouse")]
        public void EditProductAsync_ProductIsNotChanged_ThrowEntityPersistFailureException
        (string? description, string? mainImageUrl, string name, int priceInt, string categoryId, string categorySlug, string productSlug)
        {
            //Arrange
            decimal price = priceInt / 100m;

            Guid guidCategoryId = Guid.Parse(categoryId);

            var product = new Product
            {
                Id = _productId,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = _categoryId3,
                Category = new Category { Slug = "A" }
            };

            var receivedDto = new ProductFormDto
            {
                Id = product.Id,
                Name = name,
                Description = description,
                Price = price,
                MainImageUrl = mainImageUrl,
                CategoryId = guidCategoryId,
            };

            var expected = new Product
            {
                Id = receivedDto.Id,
                Name = receivedDto.Name,
                Description = receivedDto.Description,
                Price = receivedDto.Price,
                MainImageUrl = receivedDto.MainImageUrl,
                CategoryId = receivedDto.CategoryId
            };
            string expectedCategorySlug = categorySlug;
            string expectedProductSlug = productSlug;
            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category
                {
                    Id = guidCategoryId,
                    Slug = categorySlug
                });

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync(product);

            productRepositoryMock.Setup(pr => pr.ProductUpdate(It.IsAny<Product>()));

            productRepositoryMock.Setup(pr => pr.SaveChangesAsync())
                .ReturnsAsync(0);

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_ProductsCategorySlugDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            var product = new Product
            {
                Id = _productId,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = _categoryId3,
                Category = new Category { Slug = null! }
            };

            var receivedDto = new ProductFormDto
            {
                Id = Guid.Parse("cf00db59-20c4-4417-8fd2-313b9b9d205e"),
                CategoryId = _categoryId3
            };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category());

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync(product);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_ProductsDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            var receivedDto = new ProductFormDto { Id = _productId, CategoryId = _categoryId3 };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category());

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync((Product)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_DtoCategoryIdDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            var receivedDto = new ProductFormDto { CategoryId = _categoryId3 };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync((Category)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void SoftDeleteProductAsync_ProductDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            var category = productRepositoryMock.Setup(pr =>
                    pr.GetProductAsync(productSlug, true, false, true))
                .ReturnsAsync((Product)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.SoftDeleteProductAsync(productSlug));
        }

        [Test]
        public void SoftDeleteProductAsync_CartItemsDoNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            Product product = new Product { Slug = productSlug };

            var category = productRepositoryMock.Setup(pr =>
                    pr.GetProductAsync(productSlug, true, false, true))
                .ReturnsAsync(product);

            var cartItems = shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByProductSlugAsync(product.Slug))
                .ReturnsAsync((IEnumerable<CartItem>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.SoftDeleteProductAsync(productSlug));
        }

        [Test]
        public async Task SoftDeleteProductAsync_ProductExistsAndIsPublished_PerformsFullCleanup()
        {
            //Arrange
            string slug = "slug";
            Product product = new Product
            {
                Slug = slug,
                IsPublished = true,
                ProductVariants = new List<ProductVariant>
                {
                    new ProductVariant { Id = _variantId1, IsDeleted = false },
                    new ProductVariant { Id = _variantId2, IsDeleted = false }
                }
            };

            var cartItems = new List<CartItem>
            {
                new CartItem { Id = _cartItemId1 },
                new CartItem { Id = _cartItemId2 }
            };

            productRepositoryMock
                .Setup(r => r.GetProductAsync(slug, true, false, true))
                .ReturnsAsync(product);

            shoppingCartRepositoryMock
                .Setup(r => r.GetCartItemsByProductSlugAsync(slug))
                .ReturnsAsync(cartItems);

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            await productService.SoftDeleteProductAsync(slug);

            //Assert
            Assert.Multiple(() =>
            {
                var variantList = product.ProductVariants.ToList();
                Assert.That(product.IsPublished, Is.False);
                Assert.That(product.Slug, Does.StartWith(slug + "-"));
                Assert.That(product.UpdatedAt, Is.EqualTo(DateOnly.FromDateTime(DateTime.UtcNow)));
                Assert.That(variantList[0].IsDeleted, Is.True);
                Assert.That(variantList[1].IsDeleted, Is.True);

                //Cart Cleanup 
                shoppingCartRepositoryMock.Verify(r =>
                        r.CartItemsRemoveRange(It.Is<IEnumerable<CartItem>>(en => en.Count() == 2)),
                    Times.Once);

                //Final Save
                productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
            });
        }

        [Test]
        public void SoftDeleteProductAsync_ProductIsAlreadyUnpublished_ThrowEntityPersistFailureException()
        {
            //Arrange
            string slug = "slug";
            var product = new Product
            {
                Slug = slug,
                IsPublished = false,
                ProductVariants = new List<ProductVariant> { new ProductVariant { Id = _variantId1, IsDeleted = true } }
            };

            var cartItems = new List<CartItem> { new() { Id = _cartItemId1 } };

            productRepositoryMock
                .Setup(r => r.GetProductAsync(slug, true, false, true))
                .ReturnsAsync(product);

            shoppingCartRepositoryMock
                .Setup(r => r.GetCartItemsByProductSlugAsync(slug))
                .ReturnsAsync(cartItems);

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);

            //Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await productService.SoftDeleteProductAsync(slug));
        }

        [Test]
        public async Task SoftDeleteProductAsync_ProductPublishedButNoCartItems_UpdatesProductAndSkipsCartRemoval()
        {
            //Arrange
            string slug = "slug";
            var product = new Product
            {
                Slug = slug,
                IsPublished = true,
                ProductVariants = new List<ProductVariant> { new ProductVariant { Id = _variantId1, IsDeleted = false } }
            };

            productRepositoryMock
                .Setup(r => r.GetProductAsync(slug, true, false, true))
                .ReturnsAsync(product);

            shoppingCartRepositoryMock
                .Setup(r => r.GetCartItemsByProductSlugAsync(slug))
                .ReturnsAsync(new List<CartItem>());

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            await productService.SoftDeleteProductAsync(slug);

            //Assert
            Assert.Multiple(() =>
            {
                //Cart Cleanup NEVER happened
                shoppingCartRepositoryMock.Verify(r =>
                        r.CartItemsRemoveRange(It.IsAny<IEnumerable<CartItem>>()),
                    Times.Never);

                Assert.That(product.IsPublished, Is.False);
                Assert.That(product.Slug, Does.Contain("-"));
                Assert.That(product.ProductVariants.First().IsDeleted, Is.True);

                //ProductUpdate was called
                productRepositoryMock.Verify(r => r.ProductUpdate(product), Times.Once);

                //Save was called
                productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
            });
        }

        [Test]
        public async Task RestoreProductAsync_ProductIsSoftDeleted_RestoresStatusAndCleansSlug()
        {
            //Arrange
            string originalSlug = "slug";
            string timestampedSlug = $"{originalSlug}-20260405-145500";

            var product = new Product
            {
                Slug = timestampedSlug,
                IsPublished = false,
                ProductVariants = new List<ProductVariant>
                {
                    new ProductVariant { Id = _variantId1, IsDeleted = true },
                    new ProductVariant { Id = _variantId2, IsDeleted = true }
                }
            };

            productRepositoryMock
                .Setup(r => r.GetProductAsync(timestampedSlug, false, false, true))
                .ReturnsAsync(product);

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            await productService.RestoreProductAsync(timestampedSlug);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(product.IsPublished, Is.True);
                Assert.That(product.UpdatedAt, Is.EqualTo(DateOnly.FromDateTime(DateTime.UtcNow)));
                Assert.That(product.Slug, Is.EqualTo(originalSlug));
                Assert.That(product.ProductVariants.All(v => v.IsDeleted == false), Is.True);

                //RestoreProduct was call
                productRepositoryMock.Verify(r => r.RestoreProduct(product), Times.Once);
            });
        }

        [Test]
        public async Task RestoreProductAsync_ProductIsNotSoftDeleted_VerifyRestoreProductNeverHappened()
        {
            //Arrange
            string originalSlug = "slug";
            string timestampedSlug = $"{originalSlug}-20260405-145500";

            var product = new Product
            {
                Slug = timestampedSlug,
                IsPublished = true,
                ProductVariants = new List<ProductVariant>
                {
                    new ProductVariant { Id = _variantId1, IsDeleted = true },
                    new ProductVariant { Id = _variantId2, IsDeleted = true }
                }
            };

            productRepositoryMock
                .Setup(r => r.GetProductAsync(timestampedSlug, false, false, true))
                .ReturnsAsync(product);

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            await productService.RestoreProductAsync(timestampedSlug);

            //Assert
            productRepositoryMock.Verify(r => r.RestoreProduct(product), Times.Never);
        }

        [Test]
        public void RestoreProductAsync_ProductDoesNotExist_ThrowsEntityNotFoundException()
        {
            //Arrange
            var fakeSlug = "slug";

            productRepositoryMock
                .Setup(r => r.GetProductAsync(fakeSlug, false, false, true))
                .ReturnsAsync((Product?)null);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.RestoreProductAsync(fakeSlug));
        }

        [Test]
        public async Task ManageProductVariantsAsync_VariantDeleted_RemovesAssociatedCartItems()
        {
            //Arrange
            var existingVariants = new List<ProductVariant>
            {
                new ProductVariant { Id = _variantId1, ProductId = _productId, VariantName = "Discontinued Size", IsDeleted = false }
            };

            var dto = new ProductVariantListDto
            {
                ProductId = _productId,
                Variants = new List<ProductVariantDto>()
            };

            var cartItems = new List<CartItem>
            {
                new CartItem { Id = _cartItemId1, ProductId = _productId, ProductVariantId = _variantId1 },
                new CartItem { Id = _cartItemId2, ProductId = _productId, ProductVariantId = _variantId1 }
            };

            productRepositoryMock
                .Setup(r => r.GetProductVariantsByProductIdAsync(_productId))
                .ReturnsAsync(existingVariants);

            shoppingCartRepositoryMock
                .Setup(r => r.GetCartItemsByProductIdAndVariantIdAsync(_productId, _variantId1))
                .ReturnsAsync(cartItems);

            productRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            await productService.ManageProductVariantsAsync(dto);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(existingVariants.First().IsDeleted, Is.True);

                shoppingCartRepositoryMock.Verify(r =>
                    r.CartItemsRemoveRange(It.Is<IEnumerable<CartItem>>(list => list.Count() == 2)),
                    Times.Once);

                productRepositoryMock.Verify(r =>
                    r.UpdateRangeProductVariantAsync(It.IsAny<IEnumerable<ProductVariant>>()),
                    Times.Once);

                productRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
            });
        }
    }
}