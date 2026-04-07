using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using Moq;

namespace MaxEndLabs.Service.Tests
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private IShoppingCartService shoppingCartService;

        private Mock<IShoppingCartRepository> shoppingCartRepositoryMock;

        private readonly Guid _userId = Guid.Parse("399a1b4e-8ec4-4203-8f63-46ee1c37cfd1");
        private readonly Guid _shoppingCartId = Guid.Parse("ac9bb5d0-7714-4e09-8070-5f62666f8322");
        private readonly Guid _productId = Guid.Parse("057e6259-55b4-4ddd-9d0f-c1b11cb1f2f0");
        private readonly Guid _variantId1 = Guid.Parse("1d3c10bc-ee5f-4423-8130-e9cd31603392");
        private readonly Guid _cartItemId1 = Guid.Parse("01c3ab5c-7f7d-4340-b28e-39fc72156472");

        [SetUp]
        public void Setup()
        {
            shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();

            shoppingCartService = new ShoppingCartService(shoppingCartRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllCartItemsAsync_HasUser_ReturnCorrectPopulatedDto()
        {
            //Arrange
            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    Id = _cartItemId1,
                    CartId = _shoppingCartId,
                    ProductId = _productId,
                    Quantity = 2,
                    Product = new Product
                    {
                        Name = "Gaming Mouse",
                        Price = 59.99m,
                        MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                    },
                    ProductVariant = new ProductVariant
                    {
                        Id = _variantId1,
                        VariantName = "Matte Black",
                        Price = 64.99m
                    }
                }
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(_userId))
                .ReturnsAsync(cartItemList);

            var expected = new ShoppingCartIndexDto
            {
                TotalPrice = 2 * 64.99m,
                CartId = _shoppingCartId,
                CartItems = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        ProductId = _productId,
                        ProductName = "Gaming Mouse",
                        ProductVariantId = _variantId1,
                        VariantName = "Matte Black",
                        UnitPrice = 64.99m,
                        MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                        Quantity = 2
                    }
                }
            };

            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(_userId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.CartId, Is.EqualTo(expected.CartId));
                Assert.That(result.TotalPrice, Is.EqualTo(expected.TotalPrice));
                Assert.That(result.CartItems.Count, Is.EqualTo(expected.CartItems.Count));
                Assert.That(result.CartItems[0].ProductId, Is.EqualTo(expected.CartItems[0].ProductId));
                Assert.That(result.CartItems[0].ProductName, Is.EqualTo(expected.CartItems[0].ProductName));
                Assert.That(result.CartItems[0].ProductVariantId, Is.EqualTo(expected.CartItems[0].ProductVariantId));
                Assert.That(result.CartItems[0].VariantName, Is.EqualTo(expected.CartItems[0].VariantName));
                Assert.That(result.CartItems[0].UnitPrice, Is.EqualTo(expected.CartItems[0].UnitPrice));
                Assert.That(result.CartItems[0].MainImageUrl, Is.EqualTo(expected.CartItems[0].MainImageUrl));
                Assert.That(result.CartItems[0].Quantity, Is.EqualTo(expected.CartItems[0].Quantity));
            });
        }

        [Test]
        public async Task GetAllCartItemsAsync_CartItemListIsEmpty_ReturnEmptyCartOrderDto()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(_userId))
                .ReturnsAsync(new List<CartItem>());

            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(_userId);

            //Assert
            Assert.That(result.CartItems.Count, Is.EqualTo(0));
        }

        [Test]
        [TestCase(100, 0, 0)]
        [TestCase(0, 100, 100)]
        [TestCase(100, null, 100)]
        [TestCase(0, null, 0)]
        public async Task GetAllCartItemsAsync_MultipleVariants_ReturnCorrectUnitPrice
            (int productPriceInt, int? variantPriceInt, int expectedUnitPriceInt)
        {
            //Arrange
            decimal productPrice = productPriceInt / 100m;
            decimal? variantPrice = variantPriceInt / 100m;
            decimal expectedUnitPrice = expectedUnitPriceInt / 100m;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    Product = new Product
                    {
                        Price = productPrice,
                    },
                    ProductVariant = new ProductVariant
                    {
                        Price = variantPrice,
                    }
                }
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(_userId))
                .ReturnsAsync(cartItemList);


            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(_userId);

            //Assert
            Assert.That(result.CartItems[0].UnitPrice, Is.EqualTo(expectedUnitPrice));
        }

        [Test]
        [TestCase(new int[] { 200, 500 }, new int[] { 1, 2 }, 1200)]
        [TestCase(new int[] { 100, 100 }, new int[] { 1, 1 }, 200)]
        [TestCase(new int[] { 0, 100 }, new int[] { 1, 1 }, 100)]
        [TestCase(new int[] { 0, 0 }, new int[] { 1, 1 }, 0)]
        public async Task GetAllCartItemsAsync_MultipleVariants_ReturnCorrectTotalPrice
            (int[] unitPriceInt, int[] quantity, int expectedTotalInt)
        {
            //Arrange
            decimal unitPriceOne = unitPriceInt[0] / 100m;
            decimal unitPriceTwo = unitPriceInt[1] / 100m;

            decimal expectedTotal = expectedTotalInt / 100m;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    Quantity = quantity[0],
                    Product = new Product { Price = unitPriceOne, },
                    ProductVariant = new ProductVariant { Price = null, }
                },
                new CartItem
                {
                    Quantity = quantity[1],
                    Product = new Product { Price = unitPriceTwo, },
                    ProductVariant = new ProductVariant { Price = null, }
                }
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(_userId))
                .ReturnsAsync(cartItemList);


            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(_userId);

            //Assert
            Assert.That(result.TotalPrice, Is.EqualTo(expectedTotal));
        }

        [Test]
        public void GetAllCartItemsAsync_CartItemsListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(_userId))
                .ReturnsAsync((List<CartItem>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.GetAllCartItemsAsync(_userId));
        }


        [Test]
        public async Task AddProductToShoppingCartAsync_CartItemIsNull_AssertNewCartItemCorrectly()
        {
            //Arrange
            CartItemCreateDto dto = new CartItemCreateDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1,
                Quantity = 1
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync((CartItem?)null);

            CartItem capturedCartItem = null!;

            shoppingCartRepositoryMock.Setup(scr => scr.AddToCartAsync(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedCartItem = ci)
                .Returns(Task.CompletedTask);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.AddProductToShoppingCartAsync(dto);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(capturedCartItem.CartId, Is.EqualTo(dto.CartId));
                Assert.That(capturedCartItem.ProductId, Is.EqualTo(dto.ProductId));
                Assert.That(capturedCartItem.ProductVariantId, Is.EqualTo(dto.ProductVariantId));
                Assert.That(capturedCartItem.Quantity, Is.EqualTo(dto.Quantity));
                Assert.That(capturedCartItem.IsPublished, Is.EqualTo(true));
            });
        }

        [Test]
        public async Task AddProductToShoppingCartAsync_CartItemExistButNotPublished_AssertCartItemChangesCorrectly()
        {
            //Arrange
            CartItemCreateDto dto = new CartItemCreateDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1,
                Quantity = 2
            };
            var initialDate = DateTime.UtcNow.AddDays(-1);

            CartItem cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                ProductVariantId = dto.ProductVariantId,
                Quantity = 1,
                AddedAt = initialDate,
                IsPublished = false
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync(cartItem);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.AddProductToShoppingCartAsync(dto);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(cartItem.Quantity, Is.EqualTo(dto.Quantity));
                Assert.That(cartItem.IsPublished, Is.EqualTo(true));
                Assert.That(cartItem.AddedAt, Is.GreaterThan(initialDate));
            });

            shoppingCartRepositoryMock.Verify(scr => scr.AddToCartAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public async Task AddProductToShoppingCartAsync_CartItemExistAndIsPublished_AssertCartItemChangesCorrectly()
        {
            //Arrange
            CartItemCreateDto dto = new CartItemCreateDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1,
                Quantity = 2
            };

            CartItem cartItem = new CartItem { Quantity = 1, IsPublished = true };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync(cartItem);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.AddProductToShoppingCartAsync(dto);

            //Assert
            Assert.That(cartItem.Quantity, Is.EqualTo(3));
            shoppingCartRepositoryMock.Verify(scr => scr.AddToCartAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public void AddProductToShoppingCartAsync_SaveFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            CartItemCreateDto dto = new CartItemCreateDto 
                { 
                    CartId = _shoppingCartId, 
                    ProductId = _productId, 
                    ProductVariantId = _variantId1,
                    Quantity = 1
                };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync((CartItem?)null);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync()).ReturnsAsync(0);

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.AddProductToShoppingCartAsync(dto));
        }

        [Test]
        public async Task RemoveCartItemFromShoppingCartAsync_CartItemExist_AssertCartItemChangesCorrectly()
        {
            //Arrange
            CartItemDeleteDto dto = new CartItemDeleteDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1
            };

            CartItem cartItem = new CartItem
            {
                IsPublished = true
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync(cartItem);

            CartItem capturedCartItem = null!;

            shoppingCartRepositoryMock.Setup(scr => scr.CartItemUpdate(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedCartItem = ci);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto);

            //Assert
            Assert.That(capturedCartItem.IsPublished, Is.EqualTo(false));
        }


        [Test]
        public void RemoveCartItemFromShoppingCartAsync_CartItemExistButIsNotPublished_ThrowEntityPersistFailureException()
        {
            //Arrange
            CartItemDeleteDto dto = new CartItemDeleteDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1
            };

            CartItem cartItem = new CartItem
            {
                IsPublished = false
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync(cartItem);

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto));
        }

        [Test]
        public void RemoveCartItemFromShoppingCartAsync_CartItemIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            CartItemDeleteDto dto = new CartItemDeleteDto
            {
                CartId = _shoppingCartId,
                ProductId = _productId,
                ProductVariantId = _variantId1
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync((CartItem?)null);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_CartItemListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(_shoppingCartId))
                .ReturnsAsync((List<CartItem>)null!);

            //Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(_shoppingCartId));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_CartItemListIsEmpty_ThrowEntityPersistFailureException()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(_shoppingCartId))
                .ReturnsAsync(new List<CartItem>());

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(_shoppingCartId));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_ItemsAreNotPublished_ThrowEntityPersistFailureException()
        {
            //Arrange
            var cartItemList = new List<CartItem>
            {
                new CartItem { IsPublished = false },
                new CartItem { IsPublished = false },
                new CartItem { IsPublished = false }
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(_shoppingCartId))
                .ReturnsAsync(cartItemList);

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(_shoppingCartId));
        }

        [Test]
        public async Task DeleteAllCartItemsFromShoppingCartAsync_ListCartItems_AssertCartItemsChangesCorrectly()
        {
            //Arrange
            var cartItemList = new List<CartItem>
            {
                new CartItem { IsPublished = true },
                new CartItem { IsPublished = false },
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(_shoppingCartId))
                .ReturnsAsync(cartItemList);

            IEnumerable<CartItem> capturedCartItems = null!;
            shoppingCartRepositoryMock.Setup(scr => scr.CartItemUpdateRange(It.IsAny<IEnumerable<CartItem>>()))
                .Callback<IEnumerable<CartItem>>(ci => capturedCartItems = ci);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(_shoppingCartId);


            //Assert
            var capturedCartItemList = capturedCartItems.ToList();
            Assert.Multiple(() =>
            {
                Assert.That(capturedCartItemList[0].IsPublished, Is.EqualTo(false));
                Assert.That(capturedCartItemList.Count(), Is.EqualTo(2));
                Assert.That(capturedCartItemList[1].IsPublished, Is.EqualTo(false));
            });
        }

        [Test]
        public async Task GetOrCreateShoppingCartAsync_ShoppingCartDoesNotExist_ReturnCreatedCartIdCorrectly()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(Guid.Empty);

            shoppingCartRepositoryMock.Setup(scr => scr.AddShoppingCartAsync(It.IsAny<ShoppingCart>()))
                .Callback<ShoppingCart>(ci => ci.Id = _shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await shoppingCartService.GetOrCreateShoppingCartAsync(_userId);

            //Assert
            Assert.That(result, Is.EqualTo(_shoppingCartId));
        }

        [Test]
        public async Task GetOrCreateShoppingCartAsync_ShoppingCartExist_ReturnCartIdCorrectly()
        {
            //Arrange
            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(_shoppingCartId);

            //Act
            var result = await shoppingCartService.GetOrCreateShoppingCartAsync(_userId);

            //Assert
            Assert.That(result, Is.EqualTo(_shoppingCartId));
        }

        [Test]
        public void GetOrCreateShoppingCartAsync_SaveFails_ThrowsEntityPersistFailureException()
        {
            // Arrange
            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(_userId))
                .ReturnsAsync(Guid.Empty);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync()).ReturnsAsync(0);

            //Act & Assert
            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.GetOrCreateShoppingCartAsync(_userId));
        }
    }
}