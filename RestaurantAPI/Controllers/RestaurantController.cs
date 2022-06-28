using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.DbContexts;
using Repository.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantServices _restaurantServices { get; }
        private IWaiterServices _waiterServices { get; }
        private IClientServices _clientServices { get; }
        private readonly IConfiguration _configuration;

        public RestaurantController(IConfiguration _config)
        {
            _configuration = _config;
            string connectionString = _configuration.GetConnectionString("myDb1");
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
            var restaurantDbContext = new RestaurantDbContext(options);
            _restaurantServices = new RestaurantServices(restaurantDbContext);
            _waiterServices = new WaiterServices(restaurantDbContext);
            _clientServices = new ClientServices(restaurantDbContext);
        }

        [HttpPost("Create a brand new restaurant with waiters and clients in it")]
        public Result CreateNewFilledRestaurant(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            return _restaurantServices.CreateRestaurantWithNewWaitersAndClients(name, address, email, phone, waitersNumber, clientsNumber);
        }

        [HttpPost("Create a brand new empty restaurant")]
        public Result CreateNewEmptyRestaurant(string name, string address, string email, string phone)
        {
            return _restaurantServices.CreateEmptyRestaurant(name, address, email, phone);
        }

        [HttpPost("Add a brand new waiter to restaurant")]
        public Result AddNewWaiterToRestaurant(Guid restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            return _waiterServices.AddNewWaiterToSpecificRestaurant(restaurantID, waiterFirstName, waiterLastName, waiterGender, waiterAge);
        }

        [HttpPost("Add a brand new waiters to restaurant")]
        public Result AddNewWaitersToRestaurant(Guid restaurantId, int waitersNumber)
        {
            return _waiterServices.AddNewDummyListOfWaitersToSpecificRestaurant(restaurantId, waitersNumber);
        }

        [HttpPost("Add a brand new client to restaurant")]
        public Result AddNewClientToRestaurant(Guid restaurantID, string clientFirstName, string clientLastName)
        {
            return _clientServices.AddNewClientToSpecificRestaurant(restaurantID, clientFirstName, clientLastName);
        }

        [HttpPost("Add a brand new clients to restaurant")]
        public Result AddNewClientsToRestaurant(Guid restaurantId, int clientsNumber)
        {
            return _clientServices.AddNewDummyListOfClientsToSpecificRestaurant(restaurantId, clientsNumber);
        }

        [HttpGet("Show all Restaurant clients")]
        public List<Client> GetRestaurantClients(Guid restaurantId)
        {
            return _restaurantServices.ShowAllSpecificRestaurantClients(restaurantId);
        }

        [HttpGet("Show all Restaurant waiters")]
        public List<Waiter> GetRestaurantWaiters(Guid restaurantId)
        {
            return _restaurantServices.ShowAllSpecificRestaurantWaiters(restaurantId);
        }

        [HttpGet("Show all clients by waiter")]
        public List<Client> GetClientsByWaiters(Guid waiterId)
        {
            return _clientServices.ShowAllClientsBySpecificWaiter(waiterId);
        }

        [HttpPut("Transfer the waiter to another restaurant")]
        public Result TransferWaiter(Guid waiterId, Guid moveIntoRestaurantId)
        {
            return _waiterServices.TransferTheWaiterToAnotherRestaurant(waiterId, moveIntoRestaurantId);
        }

        [HttpPut("Transfer the client to another restaurant")]
        public Result TransferClient(Guid clientId, Guid moveIntoRestaurantId)
        {
            return _clientServices.TransferTheClientToAnotherRestaurant(clientId, moveIntoRestaurantId);
        }

        [HttpDelete("Delete the restaurant")]
        public Result Delete(Guid restaurantId)
        {
            return _restaurantServices.DeleteRestaurant(restaurantId);
        }

        [HttpDelete("Delete the waiter")]
        public Result DeleteWaiter(Guid waiterId)
        {
            return _waiterServices.DeleteTheWaiter(waiterId);
        }

        [HttpDelete("Delete the client")]
        public Result DeleteClient(Guid clientId)
        {
            return _clientServices.DeleteTheClient(clientId);
        }
    }
}
