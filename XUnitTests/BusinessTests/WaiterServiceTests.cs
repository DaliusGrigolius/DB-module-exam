using Business;
using Business.Services;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTests.BusinessTests
{
    public class WaiterServiceTests : IDisposable
    {
        private WaiterServices Ws { get; }
        private RestaurantDbContext Rdbc { get; set; }

        public WaiterServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("WaiterTestDb").Options;
            Rdbc = new RestaurantDbContext(dbContextOptions);
            Rdbc.Database.EnsureCreated();
            Ws = new WaiterServices(Rdbc);
        }

        [Fact]
        public void AddNewWaiterToSpecificRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = Ws.AddNewWaiterToSpecificRestaurant(new Guid(), "name", "surname", "gender", 20);
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewWaiterToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.SaveChanges();
            var actual = Ws.AddNewWaiterToSpecificRestaurant(rest.Id, "name", "surname", "gender", 20);
            var expected = new Result(true, "Success: New waiter added.");

            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count > 0);
        }

        [Fact]
        public void TransferTheWaiterToAnotherRestaurant_WaiterDoesntExist_ReturnsFalse()
        {
            var actual = Ws.TransferTheWaiterToAnotherRestaurant(new Guid(), new Guid());
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
            Rdbc.Add(rest);
            Rdbc.SaveChanges();
            var actual = Ws.TransferTheWaiterToAnotherRestaurant(waiter.Id, new Guid());
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
            Rdbc.Add(rest);
            Rdbc.Add(rest1);
            Rdbc.SaveChanges();

            var actual = Ws.TransferTheWaiterToAnotherRestaurant(rest.Waiters[0].Id, rest1.Id);
            var expected = new Result(true, "Success: Waiter transfered.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count == 0);
            Assert.True(rest1.Waiters.Count == 1);
        }

        [Fact]
        public void DeleteTheWaiter_WaiterDoesntExist_ReturnsFalse()
        {
            var actual = Ws.DeleteTheWaiter(new Guid());
            var expected = new Result(false, "Error: Waiter not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteTheWaiter_RemovesWaiter_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            rest.Waiters.Add(new Waiter("name", "surname", "gender", 20));
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = Ws.DeleteTheWaiter(rest.Waiters[0].Id);
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
            var actual = Ws.AddNewDummyListOfWaitersToSpecificRestaurant(new Guid(), 5);
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void AddNewDummyListOfWaitersToSpecificRestaurant_AddsData_ReturnsTrue()
        {
            var rest = new Restaurant("a", "b", "c", "d");
            Rdbc.Add(rest);
            Rdbc.SaveChanges();

            var actual = Ws.AddNewDummyListOfWaitersToSpecificRestaurant(rest.Id, 5);
            var expected = new Result(true, $"Success: Waiters added.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
            Assert.True(rest.Waiters.Count == 5);
        }

        public void Dispose()
        {
            Rdbc.Database.EnsureDeleted();
        }
    }
}
