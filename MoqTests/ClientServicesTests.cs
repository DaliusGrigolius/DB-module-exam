using Business.Interfaces;
using Moq;
using Repository.DbConfigs;
using Repository.Entities;
using RestaurantAPI.Controllers;
using Xunit;

namespace MoqTests
{
    public class ClientServicesTests
    {
        private readonly RestaurantController controller;
        private Mock<IRestaurantServices> restaurantServicesMock;
        private Mock<IWaiterServices> waiterServicesMock;
        private Mock<IClientServices> clientServicesMock;

        public ClientServicesTests()
        {
            restaurantServicesMock = new Mock<IRestaurantServices>();
            waiterServicesMock = new Mock<IWaiterServices>();
            clientServicesMock = new Mock<IClientServices>();

            //configurationMock.Setup(i => i.ConnectionString).Returns("ClientsDb");

            controller = new RestaurantController(restaurantServicesMock.Object, waiterServicesMock.Object, clientServicesMock.Object);
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            //Arrange
            var restultObj = new Result(false, "Error: Restaurant not found.");
            clientServicesMock.Setup(i => i.AddNewClientToSpecificRestaurant(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(restultObj);

            //Act
            var result = controller.AddNewClientToRestaurant(It.IsAny<Guid>(), "a", "b");

            // Assert
            clientServicesMock.Verify(s => s.AddNewClientToSpecificRestaurant(It.IsAny<Guid>(), "a", "b"), Times.Once());
            Assert.Equal(restultObj, result);
        }
    }
}