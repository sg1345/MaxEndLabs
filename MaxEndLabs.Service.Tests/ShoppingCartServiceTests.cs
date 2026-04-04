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
            string userId = "user123";

            int shoppingCartId = 1;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    CartId = shoppingCartId,
                    ProductId = 50,
                    Quantity = 2,
                    Product = new Product
                    {
                        Name = "Gaming Mouse",
                        Price = 59.99m,
                        MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                    },
                    ProductVariant = new ProductVariant
                    {
                        Id = 100,
                        VariantName = "Matte Black",
                        Price = 64.99m
                    }
                }
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(cartItemList);

            var expected = new ShoppingCartIndexDto
            {
                TotalPrice = 2 * 64.99m,
                CartId = shoppingCartId,
                CartItems = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        ProductId = 50,
                        ProductName = "Gaming Mouse",
                        ProductVariantId = 100,
                        VariantName = "Matte Black",
                        UnitPrice = 64.99m,
                        MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                        Quantity = 2
                    }
                }
            };

            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(userId);

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
            string userId = "user123";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<CartItem>());

            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(userId);

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
            string userId = "user123";
            int shoppingCartId = 1;

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

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(cartItemList);


            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(userId);

            //Assert

            Assert.That(result.CartItems[0].UnitPrice, Is.EqualTo(expectedUnitPrice));

        }

        [Test]
        [TestCase(new int[] { 200, 500 }, new int[] { 1, 2 }, 1200)]
        [TestCase(new int[] { 100, 100 }, new int[] { 1, 1 }, 200)]
        [TestCase(new int[] { 0, 100 }, new int[] { 1, 1 }, 100)]
        [TestCase(new int[] { 0, 0 }, new int[] { 1, 1 }, 0)]
        public async Task GetAllCartItemsAsync_MultipleVariants_ReturnCorrectTotalPrice
            (int[] UnitPriceInt, int[] quantity, int expectedTotalInt)
        {
            //Arrange
            string userId = "user123";
            int shoppingCartId = 1;

            decimal unitPriceOne = UnitPriceInt[0] / 100m;
            decimal unitPriceTwo = UnitPriceInt[1] / 100m;

            decimal expectedTotal = expectedTotalInt / 100m;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    Quantity = quantity[0],
                    Product = new Product
                    {
                        Price = unitPriceOne,

                    },
                    ProductVariant = new ProductVariant
                    {
                        Price = null,
                    }
                },
                new CartItem
                {
                    Quantity = quantity[1],
                    Product = new Product
                    {
                        Price = unitPriceTwo,
                    },
                    ProductVariant = new ProductVariant
                    {
                        Price = null,
                    }
                }
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(cartItemList);


            //Act
            var result = await shoppingCartService.GetAllCartItemsAsync(userId);

            //Assert

            Assert.That(result.TotalPrice, Is.EqualTo(expectedTotal));

        }

        [Test]
        public async Task GetAllCartItemsAsync_CartItemsListIsNull_ThrowEntityNotFoundException()
        {
            string userId = "user123";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);


            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync((List<CartItem>)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.GetAllCartItemsAsync(userId));
        }


        [Test]
        public async Task AddProductToShoppingCartAsync_CartItemIsNull_AssertNewCartItemCorrectly()
        {
            //Arrange
            CartItemCreateDto dto = new CartItemCreateDto
            {
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1,
                Quantity = 1
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.Quantity))
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
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1,
                Quantity = 2
            };

            CartItem cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                ProductVariantId = dto.ProductVariantId,
                Quantity = 1,
                AddedAt = DateTime.UtcNow,
                IsPublished = false
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.Quantity))
                .ReturnsAsync(cartItem);

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
                Assert.That(capturedCartItem.AddedAt, Is.Not.EqualTo(cartItem.AddedAt));
            });
        }

        [Test]
        public async Task AddProductToShoppingCartAsync_CartItemExistAndIsPublished_AssertCartItemChangesCorrectly()
        {
            //Arrange
            CartItemCreateDto dto = new CartItemCreateDto
            {
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1,
                Quantity = 2
            };

            CartItem cartItem = new CartItem
            {
                Quantity = 1,
                IsPublished = true
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemIgnoreFilterAsync(dto.CartId, dto.ProductId, dto.Quantity))
                .ReturnsAsync(cartItem);

            CartItem capturedCartItem = null!;

            shoppingCartRepositoryMock.Setup(scr => scr.AddToCartAsync(It.IsAny<CartItem>()))
                .Callback<CartItem>(ci => capturedCartItem = ci)
                .Returns(Task.CompletedTask);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.AddProductToShoppingCartAsync(dto);

            //Assert
            Assert.That(capturedCartItem.Quantity, Is.EqualTo(dto.Quantity));
        }

        [Test]
        public async Task RemoveCartItemFromShoppingCartAsync_CartItemExist_AssertCartItemChangesCorrectly()
        {
            //Arrange
            CartItemDeleteDto dto = new CartItemDeleteDto
            {
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1
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
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1
            };

            CartItem cartItem = new CartItem
            {
                IsPublished = false
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync(cartItem);

            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto));


        }

        [Test]
        public void RemoveCartItemFromShoppingCartAsync_CartItemIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            CartItemDeleteDto dto = new CartItemDeleteDto
            {
                CartId = 1,
                ProductId = 1,
                ProductVariantId = 1
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemAsync(dto.CartId, dto.ProductId, dto.ProductVariantId))
                .ReturnsAsync((CartItem?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.RemoveCartItemFromShoppingCartAsync(dto));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_CartItemListIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            int cartId = 1;

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync((List<CartItem>)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_CartItemListIsEmpty_ThrowEntityPersistFailureException()
        {
            //Arrange
            int cartId = 1;

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync(new List<CartItem>());



            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId));
        }

        [Test]
        public void DeleteAllCartItemsFromShoppingCartAsync_ItemsAreNotPublished_ThrowEntityPersistFailureException()
        {
            //Arrange
            int cartId = 1;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    IsPublished = false
                },
                new CartItem
                {
                    IsPublished = false
                },
                new CartItem
                {
                    IsPublished = false
                }
            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync(cartItemList);

            Assert.ThrowsAsync<EntityPersistFailureException>(async () =>
                await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId));
        }

        [Test]
        public async Task DeleteAllCartItemsFromShoppingCartAsync_ListCartItems_AssertCartItemsChangesCorrectly()
        {
            //Arrange
            int cartId = 1;

            var cartItemList = new List<CartItem>
            {
                new CartItem
                {
                    IsPublished = true
                },
                new CartItem
                {
                    IsPublished = false
                },

            };

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetCartItemsByCartIdAsync(cartId))
                .ReturnsAsync(cartItemList);

            IEnumerable<CartItem> capturedCartItems = null!;
            shoppingCartRepositoryMock.Setup(scr => scr.CartItemUpdateRange(It.IsAny<IEnumerable<CartItem>>()))
                .Callback<IEnumerable<CartItem>>(ci => capturedCartItems = ci);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await shoppingCartService.DeleteAllCartItemsFromShoppingCartAsync(cartId);


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
            string userId = "user123";
            int expectedCartId =67;

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(0);

            shoppingCartRepositoryMock.Setup(scr => scr.AddShoppingCartAsync(It.IsAny<ShoppingCart>()))
                .Callback<ShoppingCart>(ci => ci.Id = expectedCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await shoppingCartService.GetOrCreateShoppingCartAsync(userId);

            //Assert
            Assert.That(result, Is.EqualTo(expectedCartId));
        }

        [Test]
        public async Task GetOrCreateShoppingCartAsync_ShoppingCartExist_ReturnCartIdCorrectly()
        {
            //Arrange
            string userId = "user123";
            int expectedCartId = 67;

            shoppingCartRepositoryMock.Setup(scr =>
                    scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(expectedCartId);

            //Act
            var result = await shoppingCartService.GetOrCreateShoppingCartAsync(userId);

            //Assert
            Assert.That(result, Is.EqualTo(expectedCartId));
        }
    }
}
