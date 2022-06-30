using Business;
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

namespace XUnitTests.BusinessTests
{
    public class WaiterServiceTests : IDisposable
    {
        private WaiterServices WaiterServices { get; }
        private RestaurantDbContext RestaurantDbContext { get; set; }
        private readonly Mock<IDbConfigurations> DbConfigurationsMock;

        public WaiterServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("WaiterTestDb").Options;
            DbConfigurationsMock = new Mock<IDbConfigurations>();
            DbConfigurationsMock.Setup(i => i.ConnectionString).Returns("WaiterTestDb");
            DbConfigurationsMock.Setup(i => i.Options).Returns(dbContextOptions);
            RestaurantDbContext = new RestaurantDbContext(DbConfigurationsMock.Object);
            WaiterServices = new WaiterServices(RestaurantDbContext);
            RestaurantDbContext.Database.EnsureCreated();
        }

        [Fact]
        public void AddNewWaiterToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = WaiterServices.AddNewWaiterToSpecificRestaurant(new Guid(), "name", "surname", "gender", 20);
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewWaiterToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            RestaurantDbContext.Add(rest);
            RestaurantDbContext.SaveChanges();
            var actual = WaiterServices.AddNewWaiterToSpecificRestaurant(rest.Id, "name", "surname", "gender", 20);
            var expected = new Result(true, "Success: New waiter added.");

            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count > 0);
        }

        [Fact]
        public void TransferTheWaiterToAnotherRestaurant_WaiterDoesntExist_ReturnsFalse()
        {
            var actual = WaiterServices.TransferTheWaiterToAnotherRestaurant(new Guid(), new Guid());
            var expected = new Result(false, "Error: Waiter not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheWaiterToAnotherRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            var waiter = new Waiter("name", "surname", "gender", 20);
            rest.Waiters.Add(waiter);
            RestaurantDbContext.Add(rest);
            RestaurantDbContext.SaveChanges();
            var actual = WaiterServices.TransferTheWaiterToAnotherRestaurant(waiter.Id, new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void TransferTheWaiterToAnotherRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            var rest1 = new Restaurant("a1", "b1", "c1", "d1");
            rest.Waiters.Add(new Waiter("name", "surname", "gender", 20));
            RestaurantDbContext.Add(rest);
            RestaurantDbContext.Add(rest1);
            RestaurantDbContext.SaveChanges();

            var actual = WaiterServices.TransferTheWaiterToAnotherRestaurant(rest.Waiters[0].Id, rest1.Id);
            var expected = new Result(true, "Success: Waiter transfered.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count == 0);
            Assert.True(rest1.Waiters.Count == 1);
        }

        [Fact]
        public void DeleteTheWaiter_WaiterDoesntExist_ReturnsFalse()
        {
            var actual = WaiterServices.DeleteTheWaiter(new Guid());
            var expected = new Result(false, "Error: Waiter not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteTheWaiter_RemovesWaiter_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            rest.Waiters.Add(new Waiter("name", "surname", "gender", 20));
            RestaurantDbContext.Add(rest);
            RestaurantDbContext.SaveChanges();

            var actual = WaiterServices.DeleteTheWaiter(rest.Waiters[0].Id);
            var expected = new Result(true, "Success: Waiter deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var waiters = new List<Waiter>();
            foreach (var waiter in rest.Waiters)
            {
                waiters.Add(waiter);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(waiters.Count == 0);
        }

        [Fact]
        public void AddNewDummyListOfWaitersToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = WaiterServices.AddNewDummyListOfWaitersToSpecificRestaurant(new Guid(), 5);
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewDummyListOfWaitersToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            RestaurantDbContext.Add(rest);
            RestaurantDbContext.SaveChanges();

            var actual = WaiterServices.AddNewDummyListOfWaitersToSpecificRestaurant(rest.Id, 5);
            var expected = new Result(true, $"Success: Waiters added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count == 5);
        }

        public void Dispose()
        {
            RestaurantDbContext.Database.EnsureDeleted();
        }
    }
}
