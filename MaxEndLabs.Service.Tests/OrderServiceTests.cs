using MaxEndLabs.Data.Models;
using MaxEndLabs.Data.Models.Enum;
using MaxEndLabs.Data.Repository.Contracts;
using MaxEndLabs.GCommon.Exceptions;
using MaxEndLabs.Service.Models.Order;
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
            Assert.Multiple(()=>
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
                .Setup(or => or.GetSearchOrdersAsync(searchTerm,searchType, 0, 2))
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
    }
}
