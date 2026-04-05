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
        }

        [Test]
        public async Task ProductExistsAsync_ProductSearchedBySlugAndId_ReturnTrue()
        {
            //Arrange
            string productName = "Test Shirt";
            int productId = 1;
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
            int productId = 1;
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
                Id = 1,
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
                        Id = 1,
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
            var category = new Category
            {
                Id = 1,
                Name = "a category",
                Slug = "a-category"
            };

            categoryRepositoryMock.Setup(cr => cr.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Category>
                {
                    category
                });

            //Act
            var result = await productService.GetAllCategoriesAsync();

            //Assert
            var resultList = result.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(resultList.Count, Is.EqualTo(1));
                Assert.That(resultList[0].Id, Is.EqualTo(category.Id));
                Assert.That(resultList[0].Name, Is.EqualTo(category.Name));
                Assert.That(resultList[0].Slug, Is.EqualTo(category.Slug));
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

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetAllCategoriesAsync());
        }

        [Test]
        public void GetAllProductsAsync_ProductListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync((List<Product>)null!);

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
                    Id = 1,
                    Name = "Ultra-Light Wireless Mouse",
                    Slug = "ultra-light-wireless-mouse",
                    Category = new Category
                    {
                        Slug = "electronics"
                    },
                    Price = 89.99m,
                    MainImageUrl = mainImageUrl,
                }
            };
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync(products);

            //Act
            var result = await productService.GetAllProductsAsync();

            //Assert
            var procutsResult = result.Products.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo("All Products"));
                Assert.That(procutsResult.Count(), Is.EqualTo(1));
                Assert.That(procutsResult[0].Id, Is.EqualTo(products[0].Id));
                Assert.That(procutsResult[0].Name, Is.EqualTo(products[0].Name));
                Assert.That(procutsResult[0].Slug, Is.EqualTo(products[0].Slug));
                Assert.That(procutsResult[0].Price, Is.EqualTo(products[0].Price));
                Assert.That(procutsResult[0].MainImageUrl, Is.EqualTo(products[0].MainImageUrl));
                Assert.That(procutsResult[0].CategorySlug, Is.EqualTo(products[0].Category.Slug));
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
                    Id = 1,
                    Name = "Ultra-Light Wireless Mouse",
                    Slug = "ultra-light-wireless-mouse",
                    Category = new Category
                    {
                        Id = 1,
                        Slug = "electronics"
                    },
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
            int categoryId = 1;

            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(categorySlug))
                .ReturnsAsync(new Category
                {
                    Id = categoryId,
                });

            productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(categoryId))
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
            int categoryId = 1;

            categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(categorySlug))
                .ReturnsAsync(new Category
                {
                    Id = categoryId,
                });
            productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(categoryId))
                .ReturnsAsync((List<Product>)null!);

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
                Id = 1,
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
                    new ProductVariant()
                    {
                        Id = 1,
                        VariantName = "Ultra white",
                        Price = 69.69m,
                        IsDeleted = false
                    },
                    new ProductVariant()
                    {
                        Id = 2,
                        VariantName = "Giga Black",
                        Price = null,
                        IsDeleted = false
                    },
                    new ProductVariant()
                    {
                        Id = 3,
                        VariantName = "Cyber Myber",
                        Price = null,
                        IsDeleted = true
                    }
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
                    new ProductVariantDto()
                    {
                        Id = 1,
                        VariantName = "Ultra white",
                        Price = 69.69m,
                    },
                    new ProductVariantDto()
                    {
                        Id = 2,
                        VariantName = "Giga Black",
                        Price = 89.99m
                    }
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

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductCreateDtoAsync());
        }

        [Test]
        public async Task GetProductCreateDtoAsync_CategoriesExist_ReturnPopulatedDto()
        {
            //Arrange
            var categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "A" },
                new Category() { Id = 2, Name = "C" },
                new Category() { Id = 3, Name = "B" }
            };

            var expectedCat = new List<Category>()
            {
                new Category() { Id = 1, Name = "A" },
                new Category() { Id = 3, Name = "B" },
                new Category() { Id = 2, Name = "C" }
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
        public async Task? AddProductAsync_CreateNewProduct_ReturnProductSlugCorrectly(string? description, string? mainImageUrl)
        {
            ProductCreateDto dto = new ProductCreateDto()
            {
                CategoryId = 1,
                Description = description,
                MainImageUrl = description,
                Name = "Product Name",
                Price = 19.99m
            };
            Product expected = new Product()
            {
                Id = 1,
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
                .Callback<Product>(p =>
                {
                    p.Id = 1;
                    capturedProduct = p;
                })
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
                Id = 1,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Category = new Category
                {
                    Name = "Electronics",
                    Slug = "electronics"
                },
                Price = 89.99m,
                ProductVariants = new List<ProductVariant>()
                {
                    new ProductVariant { Id = 1, VariantName = "Ultra white", Price = 69.69m, IsDeleted = false},
                    new ProductVariant { Id = 2, VariantName = "Giga Black", Price = null, IsDeleted = false },
                    new ProductVariant { Id = 3, VariantName = "Cyber Myber", Price = null, IsDeleted = true }
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
                    new ProductVariantDto { Id = 1, VariantName = "Ultra white", Price = 69.69m },
                    new ProductVariantDto { Id = 2, VariantName = "Giga Black", Price = null }
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

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductAsync(productSlug, true));
        }

        [Test]
        public void ManageProductVariantsAsync_ProductDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            ProductVariantListDto dto = new ProductVariantListDto()
            {
                ProductId = 1,
            };

            productRepositoryMock.Setup(pr => pr.GetProductVariantsByProductIdAsync(dto.ProductId))
                .ReturnsAsync((IEnumerable<ProductVariant>)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.ManageProductVariantsAsync(dto));
        }

        [Test]
        public async Task ManageProductVariantsAsync_ShouldHandle_DeleteUpdateAndAdd()
        {
            //Arrange
            int productId = 10;

            var existingVariants = new List<ProductVariant>
            {
                new ProductVariant { Id = 1, ProductId = productId, VariantName = "Old - To Delete", IsDeleted = false },
                new ProductVariant { Id = 2, ProductId = productId, VariantName = "Old - To Update", IsDeleted = false, Price = 10m }
            };

            var dto = new ProductVariantListDto
            {
                ProductId = productId,
                Variants = new List<ProductVariantDto>
                {
                    new ProductVariantDto { Id = 2, VariantName = "Updated Name", Price = 20m },
                    new ProductVariantDto { Id = 0, VariantName = "Brand New", Price = 30m }
                }
            };

            productRepositoryMock
                .Setup(r => r.GetProductVariantsByProductIdAsync(productId))
                .ReturnsAsync(existingVariants);

            IEnumerable<ProductVariant> capturedDeleted = null!;
            productRepositoryMock
                .Setup(r => r.UpdateRangeProductVariantAsync(It.IsAny<IEnumerable<ProductVariant>>()))
                .Callback<IEnumerable<ProductVariant>>(pvl => capturedDeleted = pvl.ToList());

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
                var deleted = capturedDeleted.First(v => v.Id == 1);
                Assert.That(deleted.IsDeleted, Is.True);

                // Update
                var updated = existingVariants.First(v => v.Id == 2);
                Assert.That(updated.VariantName, Is.EqualTo("Updated Name"));
                Assert.That(updated.Price, Is.EqualTo(20m));

                // Add
                productRepositoryMock.Verify(r => r.AddProductVariantAsync(
                    It.Is<ProductVariant>(v => v.VariantName == "Brand New" && v.Price == 30m)
                ), Times.Once);

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

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await productService.GetProductEditDtoAsync(productSlug));
        }


        [Test]
        public void GetProductEditDtoAsync_CategoriesDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            categoryRepositoryMock.Setup(pr =>
                    pr.GetAllCategoriesAsync())
                .ReturnsAsync((IEnumerable<Category>)null!);

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
                Id = 1,
                Name = "Ultra-Light Wireless Mouse",
                Slug = "ultra-light-wireless-mouse",
                Price = 89.99m,
                Description = description,
                MainImageUrl = mainImageUrl
            };
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "B" },
                new Category { Id = 2, Name = "C" },
                new Category { Id = 3, Name = "A" }
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
                    new CategorySelectDto{ Id = 3, Name = "A" },
                    new CategorySelectDto{ Id = 1, Name = "B" },
                    new CategorySelectDto{ Id = 2, Name = "C" }
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
        [TestCase(null, "Some Img", "Mouse", 8999, 3, "A", "mouse")]
        [TestCase("Bad Mouse", "Some Img", "Mouse", 8999, 3, "A", "mouse")]
        [TestCase("Good Mouse", null, "Mouse", 8999, 3, "A", "mouse")]
        [TestCase("Good Mouse", "Other Img", "Mouse", 8999, 3, "A", "mouse")]
        [TestCase("Good Mouse", "Some Img", "New Mouse", 8999, 3, "A", "new-mouse")]
        [TestCase("Good Mouse", "Some Img", "Mouse", 1999, 3, "A", "mouse")]
        [TestCase("Good Mouse", "Some Img", "Mouse", 8999, 2, "B", "mouse")]
        public async Task EditProductAsync_ProductAndCategoriesExist_ReturnPopulatedDto
            (string? description, string? mainImageUrl, string name, int priceInt, int categoryId, string categorySlug, string productSlug)
        {
            //Arrange
            decimal price = priceInt / 100m;
            var product = new Product
            {
                Id = 1,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = 3,
                Category = new Category { Slug = "A" }
            };

            var receivedDto = new ProductFormDto
            {
                Id = product.Id,
                Name = name,
                Description = description,
                Price = price,
                MainImageUrl = mainImageUrl,
                CategoryId = categoryId,
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
                    Id = categoryId,
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
        [TestCase("Good Mouse", "Some Img", "Mouse", 8999, 3, "A", "mouse")]
        public void EditProductAsync_ProductIsNotChanged_ThrowEntityPersistFailureException
    (string? description, string? mainImageUrl, string name, int priceInt, int categoryId, string categorySlug, string productSlug)
        {
            //Arrange
            decimal price = priceInt / 100m;
            var product = new Product
            {
                Id = 1,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = 3,
                Category = new Category { Slug = "A" }
            };

            var receivedDto = new ProductFormDto
            {
                Id = product.Id,
                Name = name,
                Description = description,
                Price = price,
                MainImageUrl = mainImageUrl,
                CategoryId = categoryId,
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
                    Id = categoryId,
                    Slug = categorySlug
                });

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync(product);

            productRepositoryMock.Setup(pr => pr.ProductUpdate(It.IsAny<Product>()));

            productRepositoryMock.Setup(pr => pr.SaveChangesAsync())
                .ReturnsAsync(0);

            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_ProductsCategorySlugDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Mouse",
                Slug = "mouse",
                Price = 89.99m,
                Description = "Good Mouse",
                MainImageUrl = "Some Img",
                CategoryId = 3,
                Category = new Category { Slug = null! }
            };

            var receivedDto = new ProductFormDto
            {
                Id = 67,
                CategoryId = 3
            };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category());

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_ProductsDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange

            var receivedDto = new ProductFormDto
            {
                Id = 67,
                CategoryId = 3
            };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync(new Category());

            productRepositoryMock.Setup(pr => pr.GetProductAsync(receivedDto.Id))
                .ReturnsAsync((Product)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.EditProductAsync(receivedDto));
        }

        [Test]
        public void EditProductAsync_DtoCategoryIdDoesNotExist_ThrowEntityNotFoundException()
        {
            //Arrange

            var receivedDto = new ProductFormDto
            {
                CategoryId = 3
            };

            var category = categoryRepositoryMock.Setup(cr => cr.GetCategoryAsync(receivedDto.CategoryId))
                .ReturnsAsync((Category)null!);


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


            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.SoftDeleteProductAsync(productSlug));
        }

        [Test]
        public void SoftDeleteProductAsync_CartItemsDoNotExist_ThrowEntityNotFoundException()
        {
            //Arrange
            string productSlug = "slug";

            Product product = new Product
            {
                Slug = productSlug
            };

            var category = productRepositoryMock.Setup(pr =>
                    pr.GetProductAsync(productSlug, true, false, true))
                .ReturnsAsync(product);

            var cartItems = shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByProductSlugAsync(product.Slug))
                .ReturnsAsync((IEnumerable<CartItem>)null!);


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
                    new ProductVariant { Id = 1, IsDeleted = false },
                    new ProductVariant { Id = 2, IsDeleted = false }
                }
            };

            var cartItems = new List<CartItem>
            {
                new CartItem { Id = 67 },
                new CartItem { Id = 69 }
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
                Assert.That(product.IsPublished, Is.False);
                Assert.That(product.Slug, Does.StartWith(slug + "-"));
                Assert.That(product.UpdatedAt, Is.EqualTo(DateOnly.FromDateTime(DateTime.UtcNow)));
                Assert.That(product.ProductVariants.All(v => v.IsDeleted), Is.True);

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
                ProductVariants = new List<ProductVariant>
                {
                    new ProductVariant { Id = 1, IsDeleted = true }
                }
            };

            var cartItems = new List<CartItem> { new() { Id = 55 } };

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
                ProductVariants = new List<ProductVariant>
                {
                    new ProductVariant { Id = 1, IsDeleted = false }
                }
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
                    new ProductVariant { Id = 1, IsDeleted = true },
                    new ProductVariant { Id = 2, IsDeleted = true }
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
                    new ProductVariant { Id = 1, IsDeleted = true },
                    new ProductVariant { Id = 2, IsDeleted = true }
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

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.RestoreProductAsync(fakeSlug));
        }
    }
}