using Business.Interfaces;
using Moq;
using Repository.Entities;
using RestaurantAPI.Controllers;
using System;
using Xunit;

namespace XUnitTests.MoqBusinessTests
{
    public class ClientServicesTests
    {
        private RestaurantController Controller { get; }
        private Mock<IRestaurantServices> _restaurantServicesMock { get; }
        private Mock<IWaiterServices> _waiterServicesMock { get; }
        private Mock<IClientServices> _clientServicesMock { get; }

        public ClientServicesTests()
        {
            _restaurantServicesMock = new Mock<IRestaurantServices>();
            _waiterServicesMock = new Mock<IWaiterServices>();
            _clientServicesMock = new Mock<IClientServices>();
            Controller = new RestaurantController(_restaurantServicesMock.Object, _waiterServicesMock.Object, _clientServicesMock.Object);
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            //Arrange
            var restultObj = new Result(false, "Error: Restaurant not found.");
            _clientServicesMock.Setup(i => i.AddNewClientToSpecificRestaurant(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(restultObj);

            //Act
            var result = Controller.AddNewClientToRestaurant(It.IsAny<Guid>(), "a", "b");

            // Assert
            _clientServicesMock.Verify(s => s.AddNewClientToSpecificRestaurant(It.IsAny<Guid>(), "a", "b"), Times.Once());
            Assert.Equal(restultObj, result);
        }
    }
}