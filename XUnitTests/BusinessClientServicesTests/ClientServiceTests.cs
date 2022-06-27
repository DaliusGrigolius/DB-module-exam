using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
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
            var actual = Cs.AddNewClientToSpecificRestaurant(new Guid(), "Daumantas", "Pavarde");
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
        public void TransferTheClientToAnotherRestaurant_DataChanges_ReturnsTrue()
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

        public void Dispose()
        {
            Rdbc.Database.EnsureDeleted();
        }
    }
}
