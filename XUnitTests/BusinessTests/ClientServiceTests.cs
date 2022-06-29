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
        private RestaurantDbContext Rdbc { get; set; }
        private readonly IClientServices _clientServices;
        private Mock<IDbConfigurations> dbConfigurationsMock;


        public ClientServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("ClientTestDb").Options;
            dbConfigurationsMock = new Mock<IDbConfigurations>();
            dbConfigurationsMock.Setup(i => i.ConnectionString).Returns("ClientTestDb");
            dbConfigurationsMock.Setup(i => i.Options).Returns(dbContextOptions);
            Rdbc = new RestaurantDbContext(dbConfigurationsMock.Object);
            _clientServices = new ClientServices(Rdbc);
            Rdbc.Database.EnsureCreated();
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
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = _clientServices.AddNewClientToSpecificRestaurant(rest.Id, "name", "surname");
            var expected = new Result(true, "Success: New client added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Clients.Count == 1);
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
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            var client = new Client("a", "b", rest.Id);
            Rdbc.Add(client);
            Rdbc.SaveChanges();
            var actual = _clientServices.TransferTheClientToAnotherRestaurant(client.Id, new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            var rest1 = new Restaurant("a1", "b1", "c1", "d1");
            Rdbc.Add(rest);
            Rdbc.Add(rest1);
            Rdbc.Add(new Client("name", "surname", rest.Id));
            Rdbc.SaveChanges();

            var actual = _clientServices.TransferTheClientToAnotherRestaurant(rest.Clients[0].Id, rest1.Id);
            var expected = new Result(true, "Success: Client transfered.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Clients.Count == 0);
            Assert.True(rest1.Clients.Count == 1);
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
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.Add(new Client("name", "surname", rest.Id));
            Rdbc.SaveChanges();

            var actual = _clientServices.DeleteTheClient(rest.Clients[0].Id);
            var expected = new Result(true, "Success: Client deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var clients = new List<Client>();
            foreach (var client in rest.Clients)
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
            var rest = new Restaurant("a", "b", "c", "d");
            var newWaiter = new Waiter("a", "b", "c", 20);
            rest.Waiters.Add(newWaiter);
            var newClient = new Client("a", "b", rest.Id);
            Rdbc.Clients.Add(newClient);
            foreach (var waiter in rest.Waiters)
            {
                waiter.Clients.Add(newClient);
            }
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

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
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = _clientServices.AddNewDummyListOfClientsToSpecificRestaurant(rest.Id, 5);
            var expected = new Result(true, $"Success: Clients added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Clients.Count == 5);
        }

        public void Dispose()
        {
            Rdbc.Database.EnsureDeleted();
        }
    }
}
