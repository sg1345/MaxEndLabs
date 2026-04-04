using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
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
        public async Task GetProductSearchAsync_Searched_ReturnsCorrectPopulatedDto()
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
                Category = new Category
                {
                    Name = "Electronics",
                    Slug = "electronics"
                },
                Price = 89.99m,
                MainImageUrl = "https://cdn.example.com/images/products/mouse-01.jpg",
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
        public async Task GetAllProductsAsync_ProductList_ReturnCorrectPopulatedDto()
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
                    MainImageUrl = "https://cdn.example.com/images/products/mouse-01.jpg",
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
        public async Task GetProductsByCategoryAsync_ProductList_ReturnCorrectPopulatedDto()
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
                    MainImageUrl = "https://cdn.example.com/images/products/mouse-01.jpg",
                }
            };
            categoryRepositoryMock.Setup(cr => cr.GetCategoryBySlugAsync(productSlug))
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

            categoryRepositoryMock.Setup(cr => cr.GetCategoryBySlugAsync(categorySlug))
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

            categoryRepositoryMock.Setup(cr => cr.GetCategoryBySlugAsync(categorySlug))
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

            categoryRepositoryMock.Setup(cr => cr.GetCategoryBySlugAsync(categorySlug))
                .ReturnsAsync((Category?)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.GetProductsByCategoryAsync(categorySlug));
        }

        [Test]
        public void GetProductsByCategoryAsync_CategoryNotFound_ThrowEntityNotFoundException()
        {
            //Arrange
            string categorySlug = "slug";

            categoryRepositoryMock.Setup(cr => cr.GetCategoryBySlugAsync(categorySlug))
                .ReturnsAsync(new Category
                {
                    Id = 0
                });

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await productService.GetProductsByCategoryAsync(categorySlug));
        }
    }
}
