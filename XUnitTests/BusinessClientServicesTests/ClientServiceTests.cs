using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTests.BusinessClientServicesTests
{
    public class ClientServiceTests : IDisposable
    {
        private ClientServices Cs { get; }
        private RestaurantDbContext Rdbc { get; set; }

        public ClientServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("RestaurantTestDb").Options;
            Rdbc = new RestaurantDbContext(dbContextOptions);
            Rdbc.Database.EnsureCreated();
            Cs = new ClientServices(Rdbc);
        }

        [Fact]
        public void AddNewClientToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = Cs.AddNewClientToSpecificRestaurant(new Guid(), "name", "surname");
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
            var actual = Cs.AddNewClientToSpecificRestaurant(rest.Id, "name", "surname");
            var expected = new Result(true, "Success: New client added.");

            var comparisonResult1 = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult1.AreEqual);
            Assert.True(rest.Clients.Count > 0);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_ClientDoesntExist_ReturnsFalse()
        {
            var actual = Cs.TransferTheClientToAnotherRestaurant(new Guid(), new Guid());
            var expected = new Result(false, "Error: Client not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheClientToAnotherRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = Cs.AddNewClientToSpecificRestaurant(new Guid(), "name", "surname");
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
            Rdbc.SaveChanges();
            Cs.AddNewClientToSpecificRestaurant(rest.Id, "name", "surname");

            var actual = Cs.TransferTheClientToAnotherRestaurant(rest.Clients[0].Id, rest1.Id);
            var expected = new Result(true, "Success: Client transfered.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Clients.Count < 1);
            Assert.True(rest1.Clients.Count > 0);
        }

        [Fact]
        public void DeleteTheClient_ClientDoesntExist_ReturnsFalse()
        {
            var actual = Cs.DeleteTheClient(new Guid());
            var expected = new Result(false, "Error: Client not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteTheClient_RemovesClient_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Cs.AddNewClientToSpecificRestaurant(rest.Id, "name", "surname");

            var actual = Cs.DeleteTheClient(rest.Clients[0].Id);
            var expected = new Result(true, "Success: Client deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllClientsBySpecificWaiter_WaiterDoesntExist_ReturnsNull()
        {
            var actual = Cs.ShowAllClientsBySpecificWaiter(new Guid());
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

            var actual = Cs.ShowAllClientsBySpecificWaiter(newWaiter.Id);
            var expected = new List<Client>();
            expected.Add(newClient);
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewDummyListOfClientsToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = Cs.AddNewDummyListOfClientsToSpecificRestaurant(new Guid(), 5);
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

            var actual = Cs.AddNewDummyListOfClientsToSpecificRestaurant(rest.Id, 5);
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
