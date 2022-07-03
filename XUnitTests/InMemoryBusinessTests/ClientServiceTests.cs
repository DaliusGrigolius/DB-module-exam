using Business.Interfaces;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.DbConfigs;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTests.BusinessClientServicesTests
{
    public class ClientServiceTests : IDisposable
    {
        private RestaurantDbContext Context { get; }
        private IClientServices _clientServices { get; }
        private Mock<IDbConfigurations> _dbConfigurationsMock { get; }


        public ClientServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("ClientTestDb").Options;
            _dbConfigurationsMock = new Mock<IDbConfigurations>();
            _dbConfigurationsMock.Setup(i => i.ConnectionString).Returns("ClientTestDb");
            _dbConfigurationsMock.Setup(i => i.Options).Returns(dbContextOptions);
            Context = new RestaurantDbContext(_dbConfigurationsMock.Object);
            _clientServices = new ClientServices(Context);
            Context.Database.EnsureCreated();
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = _clientServices.AddNewClientToSpecificRestaurant(new Guid(), "name", "surname");
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            Context.SaveChanges();

            var actual = _clientServices.AddNewClientToSpecificRestaurant(restaurant.Id, "name", "surname");
            var expected = new Result(true, "Success: New client added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurant.Clients.Count == 1);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_ClientDoesntExist_ReturnsFalse()
        {
            var actual = _clientServices.TransferTheClientToAnotherRestaurant(new Guid(), new Guid());
            var expected = new Result(false, "Error: Client not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            var client = new Client("a", "b", restaurant.Id);
            Context.Add(client);
            Context.SaveChanges();
            var actual = _clientServices.TransferTheClientToAnotherRestaurant(client.Id, new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_AddsData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            var otherRestaurant = new Restaurant("a1", "b1", "c1", "d1");
            Context.Add(restaurant);
            Context.Add(otherRestaurant);
            Context.Add(new Client("name", "surname", restaurant.Id));
            Context.SaveChanges();

            var actual = _clientServices.TransferTheClientToAnotherRestaurant(restaurant.Clients[0].Id, otherRestaurant.Id);
            var expected = new Result(true, "Success: Client transfered.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurant.Clients.Count == 0);
            Assert.True(otherRestaurant.Clients.Count == 1);
        }

        [Fact]
        public void DeleteTheClient_ClientDoesntExist_ReturnsFalse()
        {
            var actual = _clientServices.DeleteTheClient(new Guid());
            var expected = new Result(false, "Error: Client not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteTheClient_RemovesClient_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            Context.Add(new Client("name", "surname", restaurant.Id));
            Context.SaveChanges();

            var actual = _clientServices.DeleteTheClient(restaurant.Clients[0].Id);
            var expected = new Result(true, "Success: Client deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var clients = new List<Client>();
            foreach (var client in restaurant.Clients)
            {
                clients.Add(client);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(clients.Count == 0);
        }

        [Fact]
        public void ShowAllClientsBySpecificWaiter_WaiterDoesntExist_ReturnsNull()
        {
            var actual = _clientServices.ShowAllClientsBySpecificWaiter(new Guid());
            List<Client> expected = null;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShowAllClientsBySpecificWaiter_HasData_ReturnsClientList()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            var newWaiter = new Waiter("a", "b", "c", 20);
            restaurant.Waiters.Add(newWaiter);
            var newClient = new Client("a", "b", restaurant.Id);
            Context.Clients.Add(newClient);
            foreach (var waiter in restaurant.Waiters)
            {
                waiter.Clients.Add(newClient);
            }
            Context.Add(restaurant);
            Context.SaveChanges();

            var actual = _clientServices.ShowAllClientsBySpecificWaiter(newWaiter.Id);
            var expected = new List<Client>();
            expected.Add(newClient);
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewDummyListOfClientsToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = _clientServices.AddNewDummyListOfClientsToSpecificRestaurant(new Guid(), 5);
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewDummyListOfClientsToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            Context.SaveChanges();

            var actual = _clientServices.AddNewDummyListOfClientsToSpecificRestaurant(restaurant.Id, 5);
            var expected = new Result(true, $"Success: Clients added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurant.Clients.Count == 5);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
