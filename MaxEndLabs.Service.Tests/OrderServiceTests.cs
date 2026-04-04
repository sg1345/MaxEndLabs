using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Models.Enum;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.Order;
using MaxEndLabs.Service.Models.ShoppingCart;
using MaxEndLabs.Services.Core;
using MaxEndLabs.Services.Core.Contracts;
using Moq;

namespace MaxEndLabs.Service.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private IOrderService orderService;

        private Mock<IShoppingCartRepository> shoppingCartRepositoryMock;
        private Mock<IOrderRepository> orderRepositoryMock;

        [SetUp]
        public void Setup()
        {
            shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
            orderRepositoryMock = new Mock<IOrderRepository>();

            orderService = new OrderService(orderRepositoryMock.Object, shoppingCartRepositoryMock.Object);
        }

        [Test]
        public async Task GetOrderForUsersAsync_UserHasOrder_ReturnsCorrectPopulatedDto()
        {
            //Arrange
            string userId = "ValidUserId";
            int page = 1;
            int pageSize = 2;


            Order order1 = new Order
            {
                Id = 1,
                UserId = "ValidUserId",
                OrderNumber = "ValidOrderNumber",
                City = "ValidCity",
                StreetAddress = "ValidAddress",
                Postcode = "ValidCode",
                Status = OrderStatus.Completed,
                TotalAmount = 67.67m,
                CreatedAt = new DateTime(2026, 4, 2, 17, 52, 55),
                UpdatedAt = new DateTime(2026, 4, 2, 17, 52, 55)
            };

            orderRepositoryMock
                .Setup(or => or.GetPageOrdersAsync(userId, 0, 2))
                .ReturnsAsync(new List<Order>()
                {
                    order1
                });
            orderRepositoryMock
                .Setup(or => or.GetCountAsync(userId))
                .ReturnsAsync(1);


            //act
            var result = await orderService.GetOrdersForUserAsync(userId, page, pageSize);

            var resultOrdersList = result.Orders.ToList();

            var expected = new OrderPaginationDto
            {
                CurrentPage = page,
                TotalPages = 1,
                Orders = new List<OrderDto>
                {
                    new OrderDto
                    {
                        Id = 1,
                        OrderNumber = "ValidOrderNumber",
                        TotalAmount = 67.67m,
                        Status = nameof(OrderStatus.Completed),
                        CreatedAt = DateOnly.FromDateTime(new DateTime(2026, 4, 2, 17, 52, 55))
                    }
                }
            };

            var expectedOrderList = expected.Orders.ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.CurrentPage, Is.EqualTo(expected.CurrentPage));
                Assert.That(result.TotalPages, Is.EqualTo(expected.TotalPages));
                Assert.That(result.HasPreviousPage, Is.EqualTo(false));
                Assert.That(result.HasNextPage, Is.EqualTo(false));
                Assert.That(result.Orders.Count(), Is.EqualTo(expected.Orders.Count()));
                Assert.That(resultOrdersList[0].Id, Is.EqualTo(expectedOrderList[0].Id));
                Assert.That(resultOrdersList[0].OrderNumber, Is.EqualTo(expectedOrderList[0].OrderNumber));
                Assert.That(resultOrdersList[0].TotalAmount, Is.EqualTo(expectedOrderList[0].TotalAmount));
                Assert.That(resultOrdersList[0].Status, Is.EqualTo(expectedOrderList[0].Status));
                Assert.That(resultOrdersList[0].CreatedAt, Is.EqualTo(expectedOrderList[0].CreatedAt));
            });
        }

        [Test]
        public async Task GetOrderForUsersAsync_UserWithoutOrder_ReturnsCorrectEmptyOrders()
        {
            //Arrange
            string userId = "InvalidUserId";
            int page = 1;
            int pageSize = 2;

            orderRepositoryMock
                .Setup(or => or.GetPageOrdersAsync(userId, 0, 2))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock
                .Setup(or => or.GetCountAsync(userId))
                .ReturnsAsync(0);

            //Act
            var result = await orderService.GetOrdersForUserAsync(userId, page, pageSize);

            //Assert
            Assert.That(result.Orders.Count(), Is.EqualTo(0));
        }

        [Test]
        [TestCase(1, 10, 0)]
        [TestCase(2, 10, 10)]
        [TestCase(3, 5, 10)]
        public async Task GetOrdersForUserAsync_VariousPages_CallsRepoWithCorrectSkip(int page, int pageSize, int expectedSkip)
        {
            // Arrange
            var userId = "user-123";

            orderRepositoryMock.Setup(r => r.GetPageOrdersAsync(userId, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Order>());

            orderRepositoryMock.Setup(r => r.GetCountAsync(userId))
                .ReturnsAsync(100);

            // Act
            var result = await orderService.GetOrdersForUserAsync(userId, page, pageSize);

            // Assert
            orderRepositoryMock.Verify(r => r.GetPageOrdersAsync(
                    userId,
                    expectedSkip,
                    pageSize),
                Times.Once);
        }

        [Test]
        [TestCase(11, 5, 3)]
        [TestCase(10, 5, 2)]
        [TestCase(0, 5, 0)]
        public async Task GetOrdersForUserAsync_MultipleScenarios_CalculatesTotalPagesCorrectly(int totalCount, int pageSize, int expectedPages)
        {
            // Arrange
            var userId = "user-123";
            orderRepositoryMock.Setup(r => r.GetPageOrdersAsync(userId, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock.Setup(r => r.GetCountAsync(userId))
                .ReturnsAsync(totalCount);

            // Act
            var result = await orderService.GetOrdersForUserAsync(userId, 1, pageSize);

            // Assert
            Assert.That(result.TotalPages, Is.EqualTo(expectedPages));
        }

        [Test]
        public void GetOrderForUsersAsync_OrdersNotFound_ThrowsEntityNotFoundException()
        {
            //Arrange
            string userId = "NoUserId";
            int page = 2;
            int pageSize = 2;

            orderRepositoryMock.Setup(r => r.GetPageOrdersAsync(userId, It.IsAny<int>(), pageSize))!
                .ReturnsAsync((IEnumerable<Order>?)null);
            orderRepositoryMock.Setup(r => r.GetCountAsync(userId))
                .ReturnsAsync(It.IsAny<int>());


            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.GetOrdersForUserAsync(userId, page, pageSize));
        }

        [Test]
        [TestCase(1, 5, 5, false, false)]
        [TestCase(1, 5, 6, false, true)]
        [TestCase(2, 5, 6, true, false)]
        [TestCase(2, 5, 11, true, true)]
        public async Task GetOrdersForUserAsync_MultipleScenarios_CheckLogicHasPreviousAndNextPage
            (int page, int pageSize, int totalCount, bool hasPrevious, bool hasNext)
        {
            //Arrange
            string userId = "user_123";
            orderRepositoryMock.Setup(r => r.GetPageOrdersAsync(userId, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock.Setup(r => r.GetCountAsync(userId))
                .ReturnsAsync(totalCount);

            //Act
            var result = await orderService.GetOrdersForUserAsync(userId, page, pageSize);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.HasPreviousPage, Is.EqualTo(hasPrevious));
                Assert.That(result.HasNextPage, Is.EqualTo(hasNext));
            });
        }

        [Test]
        public async Task GetOrderSearchAsync_Searched_ReturnsCorrectPopulatedDto()
        {
            //Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            int page = 1;
            int pageSize = 2;


            Order order1 = new Order
            {
                Id = 1,
                UserId = "ValidUserId",
                OrderNumber = "ValidOrderNumber",
                City = "ValidCity",
                StreetAddress = "ValidAddress",
                Postcode = "ValidCode",
                Status = OrderStatus.Completed,
                TotalAmount = 67.67m,
                CreatedAt = new DateTime(2026, 4, 2, 17, 52, 55),
                UpdatedAt = new DateTime(2026, 4, 2, 17, 52, 55)
            };

            orderRepositoryMock
                .Setup(or => or.GetSearchOrdersAsync(searchType, searchTerm, 0, 2))
                .ReturnsAsync(new List<Order>()
                {
                    order1
                });
            orderRepositoryMock
                .Setup(or => or.GetCountAsync(searchType, searchTerm))
                .ReturnsAsync(1);


            //act
            var result = await orderService.GetOrderSearchAsync(searchTerm, searchType, page, pageSize);

            var resultOrdersList = result.Orders.ToList();

            var expected = new OrderPaginationDto
            {
                CurrentPage = page,
                TotalPages = 1,
                HasPreviousPage = false,
                HasNextPage = false,
                Orders = new List<OrderDto>
                {
                    new OrderDto
                    {
                        Id = 1,
                        OrderNumber = "ValidOrderNumber",
                        TotalAmount = 67.67m,
                        Status = nameof(OrderStatus.Completed),
                        CreatedAt = DateOnly.FromDateTime(new DateTime(2026, 4, 2, 17, 52, 55))
                    }
                }
            };

            var expectedOrderList = expected.Orders.ToList();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.CurrentPage, Is.EqualTo(expected.CurrentPage));
                Assert.That(result.TotalPages, Is.EqualTo(expected.TotalPages));
                Assert.That(result.HasPreviousPage, Is.EqualTo(expected.HasPreviousPage));
                Assert.That(result.HasNextPage, Is.EqualTo(expected.HasNextPage));
                Assert.That(result.Orders.Count(), Is.EqualTo(expected.Orders.Count()));
                Assert.That(resultOrdersList[0].Id, Is.EqualTo(expectedOrderList[0].Id));
                Assert.That(resultOrdersList[0].OrderNumber, Is.EqualTo(expectedOrderList[0].OrderNumber));
                Assert.That(resultOrdersList[0].TotalAmount, Is.EqualTo(expectedOrderList[0].TotalAmount));
                Assert.That(resultOrdersList[0].Status, Is.EqualTo(expectedOrderList[0].Status));
                Assert.That(resultOrdersList[0].CreatedAt, Is.EqualTo(expectedOrderList[0].CreatedAt));
            });
        }

        [Test]
        public async Task GetOrderSearchAsync_SearchedWithWrongTerm_ReturnsCorrectEmptyOrders()
        {
            //Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            int page = 1;
            int pageSize = 2;

            orderRepositoryMock
                .Setup(or => or.GetSearchOrdersAsync(searchTerm, searchType, 0, 2))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock
                .Setup(or => or.GetCountAsync(searchTerm, searchType))
                .ReturnsAsync(0);

            //Act
            var result = await orderService.GetOrderSearchAsync(searchTerm, searchType, page, pageSize);

            //Assert
            Assert.That(result.Orders.Count(), Is.EqualTo(0));
        }

        [Test]
        [TestCase(1, 10, 0)]
        [TestCase(2, 10, 10)]
        [TestCase(3, 5, 10)]
        public async Task GetOrderSearchAsync_VariousPages_CallsRepoWithCorrectSkip(int page, int pageSize, int expectedSkip)
        {
            // Arrange
            string searchTerm = "Something";
            string searchType = "someType";

            orderRepositoryMock.Setup(r => r.GetSearchOrdersAsync(searchType, searchTerm, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Order>());

            orderRepositoryMock.Setup(r => r.GetCountAsync(searchType, searchTerm))
                .ReturnsAsync(100);

            // Act
            var result = await orderService.GetOrderSearchAsync(searchTerm, searchType, page, pageSize);

            // Assert
            orderRepositoryMock.Verify(r => r.GetSearchOrdersAsync(
                    searchType,
                    searchTerm,
                    expectedSkip,
                    pageSize),
                Times.Once);
        }

        [Test]
        [TestCase(11, 5, 3)]
        [TestCase(10, 5, 2)]
        [TestCase(0, 5, 0)]
        public async Task GetOrderSearchAsync_MultipleScenarios_CalculatesTotalPagesCorrectly
            (int totalCount, int pageSize, int expectedPages)
        {
            // Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            orderRepositoryMock.Setup(r => r.GetSearchOrdersAsync(searchTerm, searchType, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock.Setup(r => r.GetCountAsync(searchType, searchTerm))
                .ReturnsAsync(totalCount);

            // Act
            var result = await orderService.GetOrderSearchAsync(searchTerm, searchType, 1, pageSize);

            // Assert
            Assert.That(result.TotalPages, Is.EqualTo(expectedPages));
        }

        [Test]
        public void GetOrderSearchAsync_OrdersNotFound_ThrowsEntityNotFoundException()
        {
            //Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            int page = 2;
            int pageSize = 2;

            orderRepositoryMock.Setup(r => r.GetSearchOrdersAsync(searchType, searchTerm, It.IsAny<int>(), pageSize))!
                .ReturnsAsync((List<Order>?)null);
            orderRepositoryMock.Setup(r => r.GetCountAsync(searchType, searchTerm))
                .ReturnsAsync(It.IsAny<int>());


            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.GetOrderSearchAsync(searchTerm, searchType, page, pageSize));
        }

        [Test]
        [TestCase(1, 5, 5, false, false)]
        [TestCase(1, 5, 6, false, true)]
        [TestCase(2, 5, 6, true, false)]
        [TestCase(2, 5, 11, true, true)]
        public async Task GetOrderSearchAsync_MultipleScenarios_CheckLogicHasPreviousAndNextPage
            (int page, int pageSize, int totalCount, bool hasPrevious, bool hasNext)
        {
            //Arrange
            string searchTerm = "Something";
            string searchType = "someType";
            orderRepositoryMock.Setup(r => r.GetSearchOrdersAsync(searchTerm, searchType, It.IsAny<int>(), pageSize))
                .ReturnsAsync(new List<Order>());
            orderRepositoryMock.Setup(r => r.GetCountAsync(searchType, searchTerm))
                .ReturnsAsync(totalCount);

            //Act
            var result = await orderService.GetOrderSearchAsync(searchTerm, searchType, page, pageSize);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.HasPreviousPage, Is.EqualTo(hasPrevious));
                Assert.That(result.HasNextPage, Is.EqualTo(hasNext));
            });
        }

        [Test]
        public async Task GetOrderCreateDtoAsync_HasUser_ReturnPopulatedDto()
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<CartItem>
                {
                    new CartItem
                    {
                        Id = 1,
                        CartId = shoppingCartId,
                        Quantity = 2,
                        AddedAt = DateTime.UtcNow,
                        IsPublished = true,
                        ProductId = 50,
                        Product = new Product
                        {
                            Id = 50,
                            Name = "Gaming Mouse",
                            Slug = "gaming-mouse-v1",
                            Description = "High-precision ergonomic mouse.",
                            CategoryId = 3,
                            Price = 59.99m,
                            MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                            CreatedAt = new DateOnly(2026, 1, 15),
                            UpdatedAt = new DateOnly(2026, 3, 10),
                            IsPublished = true
                        },
                        ProductVariantId = 101,
                        ProductVariant = new ProductVariant
                        {
                            Id = 101,
                            ProductId = 50,
                            VariantName = "Matte Black",
                            Price = 64.99m,
                            IsDeleted = false
                        }
                    }
                });

            //Act
            var result = await orderService.GetOrderCreateDtoAsync(userId);

            OrderCreateDto expected = new OrderCreateDto
            {
                CartId = shoppingCartId,
                TotalPrice = 2 * 64.99m,
                CartItems = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        ProductId = 50,
                        ProductName = "Gaming Mouse",
                        ProductVariantId = 101,
                        VariantName = "Matte Black",
                        UnitPrice = 64.99m,
                        MainImageUrl = "https://cdn.example.com/images/mouse.jpg",
                        Quantity = 2
                    }
                }
            };

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
        public async Task GetOrderCreateDtoAsync_HasUserNoCartItems_ReturnCorrectCout()
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<CartItem>());

            //Act
            var result = await orderService.GetOrderCreateDtoAsync(userId);

            OrderCreateDto expected = new OrderCreateDto
            {
                CartItems = new List<CartItemDto>()
            };

            //Assert
            Assert.That(result.CartItems.Count, Is.EqualTo(expected.CartItems.Count));
        }

        [Test]
        [TestCase(0, 2, 0)]
        [TestCase(1, 2, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(2, 2, 2)]
        [TestCase(0, 0, 0)]
        [TestCase(null, 0, 0)]
        [TestCase(null, 1, 1)]
        public async Task GetOrderCreateDtoAsync_MultipleVariants_ReturnCorrectUnitPrice(decimal? variantPrice, decimal productPrice, decimal expectedPrice)
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<CartItem>
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
                });

            //Act
            var result = await orderService.GetOrderCreateDtoAsync(userId);

            //Assert
            Assert.That(result.CartItems[0].UnitPrice, Is.EqualTo(expectedPrice));
        }

        [Test]
        [TestCase(new int[] { 100, 100 }, new int[] { 1, 1 }, 200)]
        [TestCase(new int[] { 200, 0 }, new int[] { 1, 1 }, 200)]
        [TestCase(new int[] { 200, 300 }, new int[] { 1, 2 }, 800)]
        public async Task GetOrderCreateDtoAsync_MultipleVariants_ReturnCorrectTotalPrice
            (int[] unitPriceIntArr, int[] quantityArr, int expectedTotalInt)
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartId = 1;

            decimal[] unitPriceDecimalArr = new decimal[2];
            decimal expectedTotal = expectedTotalInt / 100m;
            for (int i = 0; i < unitPriceIntArr.Length; i++)
            {
                unitPriceDecimalArr[i] = unitPriceIntArr[i] / 100m;
            }

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync(new List<CartItem>
                {
                    new CartItem
                    {
                        Quantity = quantityArr[0],

                        Product = new Product
                        {
                            Price = unitPriceDecimalArr[0],
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = null
                        }
                    },
                    new CartItem
                    {
                        Quantity = quantityArr[1],
                        Product = new Product
                        {
                            Price = unitPriceDecimalArr[1],
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = null
                        }
                    }
                });

            //Act
            var result = await orderService.GetOrderCreateDtoAsync(userId);

            //Assert
            Assert.That(result.TotalPrice, Is.EqualTo(expectedTotal));
        }

        [Test]
        public void GetOrderCreateDtoAsync_UserHasNoCart_ThrowsEntityNotFoundException()
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartIdNotFound = 0;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartIdNotFound);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.GetOrderCreateDtoAsync(userId));
        }

        [Test]
        public void GetOrderCreateDtoAsync_UsersCartItemAreNull_ThrowsEntityNotFoundException()
        {
            //Arrange
            string userId = "ValidUserId";
            int shoppingCartId = 1;

            shoppingCartRepositoryMock.Setup(scr => scr.GetShoppingCartIdAsync(userId))
                .ReturnsAsync(shoppingCartId);

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(userId))
                .ReturnsAsync((List<CartItem>)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.GetOrderCreateDtoAsync(userId));
        }

        [Test]
        public async Task CreateOrderAsync_HasUser_ReturnOrderId()
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>()
                {
                    new CartItem
                    {
                        Id = 1,
                        CartId = 1,
                        Quantity = 1,
                        AddedAt = DateTime.UtcNow,
                        IsPublished = true,
                        ProductId = 1,
                        Product = new Product
                        {
                            Id = 1,
                            Name = "Default Name",
                            Slug = "default-slug",
                            Description = "Default Description",
                            CategoryId = 1,
                            Price = 10.00m,
                            MainImageUrl = "http://example.com/image.png",
                            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                            UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                            IsPublished = true
                        },
                        ProductVariantId = 1,
                        ProductVariant = new ProductVariant
                        {
                            Id = 1,
                            ProductId = 1,
                            VariantName = "Default Variant Name",
                            Price = 15.00m,
                            IsDeleted = false
                        }
                    }
                });

            orderRepositoryMock.Setup(or => or.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order =>
                {
                    order.Id = 1;
                })
                .Returns(Task.CompletedTask);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await orderService.CreateOrderAsync(orderDto);

            //Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void CreateOrderAsync_HasUserWithoutCartItems_ThrowsEntityNotFoundException()
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>());

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.CreateOrderAsync(orderDto));
        }

        [Test]
        public void CreateOrderAsync_CartItemsAreNull_ThrowsEntityNotFoundException()
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync((List<CartItem>)null!);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.CreateOrderAsync(orderDto));
        }

        [Test]
        public void CreateOrderAsync_NoChangesWereMade_ThrowsEntityPersistFailureException()
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>()
                {
                    new CartItem
                    {
                        Id = 1,
                        CartId = 1,
                        Quantity = 1,
                        AddedAt = DateTime.UtcNow,
                        IsPublished = true,
                        ProductId = 1,
                        Product = new Product
                        {
                            Id = 1,
                            Name = "Default Name",
                            Slug = "default-slug",
                            Description = "Default Description",
                            CategoryId = 1,
                            Price = 10.00m,
                            MainImageUrl = "http://example.com/image.png",
                            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                            UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                            IsPublished = true
                        },
                        ProductVariantId = 1,
                        ProductVariant = new ProductVariant
                        {
                            Id = 1,
                            ProductId = 1,
                            VariantName = "Default Variant Name",
                            Price = 15.00m,
                            IsDeleted = false
                        }
                    }
                });

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(0);

            Assert.ThrowsAsync<EntityPersistFailureException>(async () => await orderService.CreateOrderAsync(orderDto));
        }

        [Test]
        [TestCase(new int[] { 1, 2 }, new object[] { 0, 100 }, new int[] { 300, 300 }, 200)]
        [TestCase(new int[] { 2, 2 }, new object[] { 100, 100 }, new int[] { 0, 300 }, 400)]
        [TestCase(new int[] { 2, 2 }, new object[] { 200, 200 }, new int[] { 100, 100 }, 800)]
        [TestCase(new int[] { 1, 1 }, new object[] { null!, 100 }, new int[] { 200, 200 }, 300)]
        [TestCase(new int[] { 2, 2 }, new object[] { null!, null! }, new int[] { 100, 100 }, 400)]
        public async Task CreateOrderAsync_OrderIsCreated_AssertTotalAmount
            (int[] quantity, object?[] variantPriceObj, int[] productPriceInt, int expectedTotalInt)
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            int itemOneQuantity = quantity[0];
            decimal? itemOneVariantPrice = (int?)variantPriceObj[0] / 100m;
            decimal itemOneProductPrice = productPriceInt[0] / 100m;

            int itemTwoQuantity = quantity[1];
            decimal? itemTwoVariantPrice = (int?)variantPriceObj[1] / 100m;
            decimal itemTwoProductPrice = productPriceInt[1] / 100m;

            decimal expectedTotal = expectedTotalInt / 100m;

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>()
                {
                    new CartItem
                    {
                        Quantity = itemOneQuantity,
                        Product = new Product
                        {
                            Price = itemOneProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemOneVariantPrice
                        }
                    },
                    new CartItem
                    {
                        Quantity = itemTwoQuantity,
                        Product = new Product
                        {
                            Price = itemTwoProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemTwoVariantPrice
                        }
                    }
                });

            Order capturedOrder = null!;

            orderRepositoryMock.Setup(or => or.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await orderService.CreateOrderAsync(orderDto);


            //Assert
            Assert.That(capturedOrder!.TotalAmount, Is.EqualTo(expectedTotal));
        }

        [Test]
        [TestCase(new int[] { 1, 2 }, new object[] { 0, 100 }, new int[] { 300, 300 }, new int[] { 0, 200 })]
        [TestCase(new int[] { 2, 2 }, new object[] { 100, 100 }, new int[] { 0, 300 }, new int[] { 200, 200 })]
        [TestCase(new int[] { 1, 1 }, new object[] { null!, 100 }, new int[] { 200, 200 }, new int[] { 200, 100 })]
        [TestCase(new int[] { 2, 2 }, new object[] { null!, null! }, new int[] { 100, 100 }, new int[] { 200, 200 })]
        public async Task CreateOrderAsync_OrderIsCreated_AssertLineTotal
    (int[] quantity, object?[] variantPriceObj, int[] productPriceInt, int[] expectedLineTotalInt)
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            int itemOneQuantity = quantity[0];
            decimal? itemOneVariantPrice = (int?)variantPriceObj[0] / 100m;
            decimal itemOneProductPrice = productPriceInt[0] / 100m;

            int itemTwoQuantity = quantity[1];
            decimal? itemTwoVariantPrice = (int?)variantPriceObj[1] / 100m;
            decimal itemTwoProductPrice = productPriceInt[1] / 100m;

            decimal expectedTotalOne = expectedLineTotalInt[0] / 100m;
            decimal expectedTotalTwo = expectedLineTotalInt[1] / 100m;

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>()
                {
                    new CartItem
                    {
                        Quantity = itemOneQuantity,
                        Product = new Product
                        {
                            Price = itemOneProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemOneVariantPrice
                        }
                    },
                    new CartItem
                    {
                        Quantity = itemTwoQuantity,
                        Product = new Product
                        {
                            Price = itemTwoProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemTwoVariantPrice
                        }
                    }
                });

            Order capturedOrder = null!;

            orderRepositoryMock.Setup(or => or.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await orderService.CreateOrderAsync(orderDto);

            var resultOrderItems = capturedOrder.OrderItems.ToList();
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultOrderItems[0].LineTotal, Is.EqualTo(expectedTotalOne));
                Assert.That(resultOrderItems[1].LineTotal, Is.EqualTo(expectedTotalTwo));
            });
        }

        [Test]
        [TestCase(new int[] { 1, 2 }, new object[] { 0, 100 }, new int[] { 300, 300 }, new int[] { 0, 100 })]
        [TestCase(new int[] { 2, 2 }, new object[] { 100, 100 }, new int[] { 0, 300 }, new int[] { 100, 100 })]
        [TestCase(new int[] { 1, 1 }, new object[] { null!, 100 }, new int[] { 200, 200 }, new int[] { 200, 100 })]
        [TestCase(new int[] { 2, 2 }, new object[] { null!, null! }, new int[] { 100, 100 }, new int[] { 100, 100 })]
        public async Task CreateOrderAsync_OrderIsCreated_AssertUnitPrice
    (int[] quantity, object?[] variantPriceObj, int[] productPriceInt, int[] expectedUnitPriceInt)
        {
            //Arrange
            AddressOrderDto orderDto = new AddressOrderDto()
            {
                UserId = "user123",
                City = "validCity",
                StreetAddress = "validAddress",
                Postcode = "validCode"
            };

            int itemOneQuantity = quantity[0];
            decimal? itemOneVariantPrice = (int?)variantPriceObj[0] / 100m;
            decimal itemOneProductPrice = productPriceInt[0] / 100m;

            int itemTwoQuantity = quantity[1];
            decimal? itemTwoVariantPrice = (int?)variantPriceObj[1] / 100m;
            decimal itemTwoProductPrice = productPriceInt[1] / 100m;

            decimal expectedPriceOne = expectedUnitPriceInt[0] / 100m;
            decimal expectedPriceTwo = expectedUnitPriceInt[1] / 100m;

            shoppingCartRepositoryMock.Setup(scr => scr.GetCartItemsByUserIdAsync(orderDto.UserId))
                .ReturnsAsync(new List<CartItem>()
                {
                    new CartItem
                    {
                        Quantity = itemOneQuantity,
                        Product = new Product
                        {
                            Price = itemOneProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemOneVariantPrice
                        }
                    },
                    new CartItem
                    {
                        Quantity = itemTwoQuantity,
                        Product = new Product
                        {
                            Price = itemTwoProductPrice
                        },
                        ProductVariant = new ProductVariant
                        {
                            Price = itemTwoVariantPrice
                        }
                    }
                });

            Order capturedOrder = null!;

            orderRepositoryMock.Setup(or => or.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            var result = await orderService.CreateOrderAsync(orderDto);

            var resultOrderItems = capturedOrder.OrderItems.ToList();
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultOrderItems[0].UnitPrice, Is.EqualTo(expectedPriceOne));
                Assert.That(resultOrderItems[1].UnitPrice, Is.EqualTo(expectedPriceTwo));
            });
        }

        [Test]
        public async Task GetOrderAsync_OrderDoesNotExist_ReturnNull()
        {
            //Arrange
            int orderId = 1;

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, false, true, false))
                .ReturnsAsync((Order?)null);

            //Act
            var result = await orderService.GetOrderAsync(orderId);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetOrderAsync_OrderExist_ReturnCorrectDto()
        {
            int orderId = 1;

            Order? order = new Order
            {
                Id = orderId,
                OrderNumber = "ORD-2024-001",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Product
                        {
                            Name = "Test Product",
                            MainImageUrl = "someImg"
                        },
                        ProductVariant = new ProductVariant
                        {
                            VariantName = "Large",

                        },
                        Quantity = 2,
                        UnitPrice = 60.00m,
                    }
                }
            };

            var expected = new StripeSessionDto
            {
                OrderId = 1,
                OrderNumber = "ORD-2024-001",
                LineItems = order.OrderItems.Select(oi => new StripeOrderItemDto
                {
                    ProductName = "Test Product",
                    VariantName = "Large",
                    Price = (long)6000m,
                    Quantity = 2,
                    ImageUrl = "someImg"
                }).ToList()
            };

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, false, true, false))
                .ReturnsAsync(order);

            //Act
            var result = await orderService.GetOrderAsync(orderId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OrderId, Is.EqualTo(expected.OrderId));
                Assert.That(result.OrderNumber, Is.EqualTo(expected.OrderNumber));
                Assert.That(result.LineItems.Count, Is.EqualTo(expected.LineItems.Count));
                Assert.That(result.LineItems[0].ProductName, Is.EqualTo(expected.LineItems[0].ProductName));
                Assert.That(result.LineItems[0].VariantName, Is.EqualTo(expected.LineItems[0].VariantName));
                Assert.That(result.LineItems[0].Price, Is.EqualTo(expected.LineItems[0].Price));
                Assert.That(result.LineItems[0].Quantity, Is.EqualTo(expected.LineItems[0].Quantity));
                Assert.That(result.LineItems[0].ImageUrl, Is.EqualTo(expected.LineItems[0].ImageUrl));
            });
        }

        [Test]
        public async Task GetOrderDetailsAsync_OrderDoesNotExist_ThrowsEntityNotFoundException()
        {
            //Arrange
            int orderId = 1;

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, false, true, true))
                .ReturnsAsync((Order?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.GetOrderDetailsAsync(orderId));
        }

        [Test]
        public async Task GetOrderDetailsAsync_OrderExist_ReturnCorrectDto()
        {
            //Arrange
            int orderId = 1;

            var order = new Order
            {
                Id = orderId,
                UserId = "User_123",
                User = new ApplicationUser
                {
                    UserName = "testuser@example.com",
                    FullName = "George Holiday"
                },
                OrderNumber = "ORD-2024-001",
                StreetAddress = "123 Developer Lane",
                City = "Tech City",
                Postcode = "12345",
                Status = OrderStatus.Pending,
                TotalAmount = 150.00m,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Product
                        {
                            Name = "Test Product",
                            MainImageUrl = "testImg"
                        },
                        ProductVariant = new ProductVariant
                        {
                            VariantName = "Large",
                        },
                        Quantity = 2,
                        UnitPrice = 60.00m,
                        LineTotal = 120.00m
                    }
                }
            };

            var statuses = new List<string>
            {
                nameof(OrderStatus.Pending),
                nameof(OrderStatus.Paid),
                nameof(OrderStatus.Shipped),
                nameof(OrderStatus.Completed),
                nameof(OrderStatus.Cancelled),
                nameof(OrderStatus.Refunded)
            };

            var expectedDto = new OrderDetailsDto
            {
                OwnerUserId = order.UserId,
                OrderId = orderId,
                OwnerFullName = order.User.FullName,
                OwnerUsername = order.User.UserName,
                OrderNumber = order.OrderNumber,
                StreetAddress = order.StreetAddress,
                City = order.City,
                Postcode = order.Postcode,
                StatusBadge = "bg-warning-subtle text-warning-emphasis border border-warning-subtle",
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                Statuses = statuses,
                LineItems = order.OrderItems.Select(oi => new OrderItemDetailsDto()
                {
                    ProductName = oi.Product.Name,
                    VariantName = oi.ProductVariant.VariantName,
                    Quantity = oi.Quantity,
                    Price = oi.UnitPrice,
                    ImageUrl = oi.Product.MainImageUrl,
                    LineTotal = oi.LineTotal
                }).ToList()
            };

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, false, true, true))
                .ReturnsAsync(order);

            //Act
            var result = await orderService.GetOrderDetailsAsync(orderId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.OwnerUserId, Is.EqualTo(expectedDto.OwnerUserId));
                Assert.That(result.OrderId, Is.EqualTo(expectedDto.OrderId));
                Assert.That(result.OwnerFullName, Is.EqualTo(expectedDto.OwnerFullName));
                Assert.That(result.OwnerUsername, Is.EqualTo(expectedDto.OwnerUsername));
                Assert.That(result.OrderNumber, Is.EqualTo(expectedDto.OrderNumber));
                Assert.That(result.StreetAddress, Is.EqualTo(expectedDto.StreetAddress));
                Assert.That(result.City, Is.EqualTo(expectedDto.City));
                Assert.That(result.Postcode, Is.EqualTo(expectedDto.Postcode));
                Assert.That(result.StatusBadge, Is.EqualTo(expectedDto.StatusBadge));
                Assert.That(result.Status, Is.EqualTo(expectedDto.Status));
                Assert.That(result.TotalAmount, Is.EqualTo(expectedDto.TotalAmount));
                Assert.That(result.CreatedAt, Is.EqualTo(expectedDto.CreatedAt));
                Assert.That(result.Statuses.Count, Is.EqualTo(expectedDto.Statuses.Count));
                Assert.That(result.LineItems.Count, Is.EqualTo(expectedDto.LineItems.Count));
                Assert.That(result.LineItems[0].ProductName, Is.EqualTo(expectedDto.LineItems[0].ProductName));
                Assert.That(result.LineItems[0].VariantName, Is.EqualTo(expectedDto.LineItems[0].VariantName));
                Assert.That(result.LineItems[0].Quantity, Is.EqualTo(expectedDto.LineItems[0].Quantity));
                Assert.That(result.LineItems[0].Price, Is.EqualTo(expectedDto.LineItems[0].Price));
                Assert.That(result.LineItems[0].ImageUrl, Is.EqualTo(expectedDto.LineItems[0].ImageUrl));
                Assert.That(result.LineItems[0].LineTotal, Is.EqualTo(expectedDto.LineItems[0].LineTotal));
            });
        }

        [Test]
        [TestCase("Pending")]
        public async Task MarkOrderAsPaidAsync_OrderStatusIsPending_ReturnCorrectStatus(string status)
        {
            //Arrange
            int orderId = 1;

            var order = new Order
            {
                Id = orderId,
                Status = Enum.Parse<OrderStatus>(status),
                User = new ApplicationUser(),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Product = new Product(),
                        ProductVariant = new ProductVariant()
                    }
                }
            };


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync(order);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);


            //Act
            var result = await orderService.MarkOrderAsPaidAsync(orderId);

            //Assert
            Assert.That(result, Is.EqualTo("Paid"));
        }

        [Test]
        [TestCase("Shipped")]
        [TestCase("Completed")]
        [TestCase("Cancelled")]
        [TestCase("Refunded")]
        public async Task MarkOrderAsPaidAsync_OrderStatusIsNotPending_NoChangesAreMade(string status)
        {
            //Arrange
            int orderId = 1;

            var order = new Order
            {
                Id = orderId,
                Status = Enum.Parse<OrderStatus>(status),
            };


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync(order);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);


            //Act
            var result = await orderService.MarkOrderAsPaidAsync(orderId);

            //Assert
            Assert.That(result, Is.EqualTo(status));
        }

        [Test]
        public void MarkOrderAsPaidAsync_OrderIsNull_ThrowsEntityNotFoundException()
        {
            //Arrange
            int orderId = 1;


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync((Order?)null);


            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.MarkOrderAsPaidAsync(orderId));
        }

        [Test]
        public void GetOrderStatusAsync_OrderIsNull_ThrowsEntityNotFoundException()
        {
            //Arrange
            int orderId = 1;


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync((Order?)null);


            Assert.ThrowsAsync<EntityNotFoundException>(async () => await orderService.MarkOrderAsPaidAsync(orderId));
        }

        [Test]
        [TestCase("Pending")]
        [TestCase("Paid")]
        [TestCase("Shipped")]
        [TestCase("Completed")]
        [TestCase("Cancelled")]
        [TestCase("Refunded")]
        public async Task GetOrderStatusAsync_HasOrderStatus_ReturnCorrectStatus(string status)
        {
            //Arrange
            int orderId = 1;

            var order = new Order
            {
                Id = orderId,
                Status = Enum.Parse<OrderStatus>(status),
            };


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync(order);


            //Act
            var result = await orderService.GetOrderStatusAsync(orderId);

            //Assert
            Assert.That(result, Is.EqualTo(status));
        }

        [Test]
        public async Task ChangeOrderStatus_StatusIsCorrect_AssertCorrectStatusChange()
        {
            //Arrange
            string newStatus = "Completed";
            int orderId = 1;
            OrderStatus orderStatus = OrderStatus.Shipped;

            Order? order = new Order
            {
                Id = orderId,
                Status = orderStatus,
                UpdatedAt = DateTime.UtcNow,
            };

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync(order);

            orderRepositoryMock.Setup(or => or.SaveChangesAsync())
                .ReturnsAsync(1);

            //Act
            await orderService.ChangeOrderStatus(newStatus, orderId);

            //Assert
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Completed));
        }

        [Test]
        public void ChangeOrderStatus_StatusIsIncorrect_ThrowBadRequestException()
        {
            //Arrange
            string newStatus = "InvalidStatus";
            int orderId = 1;
            OrderStatus orderStatus = OrderStatus.Shipped;

            Order? order = new Order
            {
                Id = orderId,
                Status = orderStatus,
                UpdatedAt = DateTime.UtcNow,
            };

            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync(order);


            Assert.ThrowsAsync<BadRequestException>(async () =>
                await orderService.ChangeOrderStatus(newStatus, orderId));
        }

        [Test]
        public void ChangeOrderStatus_OrderIsNull_ThrowEntityNotFoundException()
        {
            //Arrange
            string newStatus = "Paid";
            int orderId = 1;


            orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId, true, false, false))
                .ReturnsAsync((Order?)null);


            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await orderService.ChangeOrderStatus(newStatus, orderId));
        }
    }
}
