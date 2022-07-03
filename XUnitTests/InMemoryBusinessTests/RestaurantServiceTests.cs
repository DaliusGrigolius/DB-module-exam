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

namespace XUnitTests.BusinessTests
{

    public class RestaurantServiceTests : IDisposable
    {
        private RestaurantDbContext Context { get; set; }
        private IRestaurantServices _restaurantServices { get; }
        private Mock<IDbConfigurations> _dbConfigurationsMock { get; }

        public RestaurantServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder().UseInMemoryDatabase("RestaurantTestDb").Options;
            _dbConfigurationsMock = new Mock<IDbConfigurations>();
            _dbConfigurationsMock.Setup(i => i.ConnectionString).Returns("RestaurantTestDb");
            _dbConfigurationsMock.Setup(i => i.Options).Returns(dbContextOptions);
            Context = new RestaurantDbContext(_dbConfigurationsMock.Object);
            _restaurantServices = new RestaurantServices(Context);
            Context.Database.EnsureCreated();
        }

        [Fact]
        public void CreateEmptyRestaurant_AddsData_ReturnsTrue()
        {
            var actual = _restaurantServices.CreateEmptyRestaurant("a", "b", "c", "d");
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restaurantsList = new List<Restaurant>();
            foreach (var rest in Context.Restaurants)
            {
                restaurantsList.Add(rest);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurantsList.Count == 1);
        }

        [Fact]
        public void CreateRestaurantWithNewWaitersAndClients_AddsData_ReturnsTrue()
        {
            var actual = _restaurantServices.CreateRestaurantWithNewWaitersAndClients("a", "b", "c", "d", 5, 5);
            var expected = new Result(true, "Success: Restaurant created.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            var restaurantsList = new List<Restaurant>();
            foreach (var restaurant in Context.Restaurants)
            {
                restaurantsList.Add(restaurant);
            }
            var waitersList = new List<Waiter>();
            foreach (var waiter in Context.Waiters)
            {
                waitersList.Add(waiter);
            }
            var clientsList = new List<Client>();
            foreach (var client in Context.Clients)
            {
                clientsList.Add(client);
            }

            Assert.True(comparisonResult.AreEqual);
            Assert.True(restaurantsList.Count == 1);
            Assert.True(waitersList.Count == 5);
            Assert.True(clientsList.Count == 5);
        }

        [Fact]
        public void DeleteRestaurant_RestaurantDoesntExist_ReturnsFalse()
        {
            var actual = _restaurantServices.DeleteRestaurant(new Guid());
            var expected = new Result(false, "Error: Restaurant not found.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void DeleteRestaurant_RemovesData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            Context.SaveChanges();

            var actual = _restaurantServices.DeleteRestaurant(restaurant.Id);
            var expected = new Result(true, "Success: Restaurant deleted.");
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantClients_HasData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            Context.Add(restaurant);
            var clientList = new List<Client>
            {
                new Client("a", "b", restaurant.Id),
                new Client("c", "d", restaurant.Id),
                new Client("e", "f", restaurant.Id),
            };
            Context.AddRange(clientList);
            Context.SaveChanges();

            var actual = _restaurantServices.ShowAllSpecificRestaurantClients(restaurant.Id);
            var expected = clientList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        [Fact]
        public void ShowAllSpecificRestaurantWaiters_HasData_ReturnsTrue()
        {
            var restaurant = new Restaurant("a", "b", "c", "d");
            var waitersList = new List<Waiter>
            {
                new Waiter("a", "b", "c", 20),
                new Waiter("a", "b", "c",18),
                new Waiter("a", "b", "c", 19),
            };
            restaurant.Waiters.AddRange(waitersList);
            Context.Add(restaurant);
            Context.SaveChanges();

            var actual = _restaurantServices.ShowAllSpecificRestaurantWaiters(restaurant.Id);
            var expected = waitersList;
            var comparisonResult = new CompareLogic().Compare(expected, actual);

            Assert.True(comparisonResult.AreEqual);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
