using Business;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private RestaurantServices Rs { get; }
        private WaiterServices Ws { get; }
        private ClientServices Cs { get; }

        public RestaurantController()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer("Server=localhost;Database=RestaurantDB;Trusted_Connection=True;").Options;
            var rdbc = new RestaurantDbContext(options);
            Rs = new(rdbc);
            Ws = new(rdbc);
            Cs = new(rdbc);
        }

        [HttpPost("Create a brand new restaurant with waiters and clients in it")]
        public Result CreateNewFilledRestaurant(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            return Rs.CreateRestaurantWithNewWaitersAndClients(name, address, email, phone, waitersNumber, clientsNumber);
        }

        [HttpPost("Create a brand new empty restaurant")]
        public Result CreateNewEmptyRestaurant(string name, string address, string email, string phone)
        {
            return Rs.CreateEmptyRestaurant(name, address, email, phone);
        }

        [HttpPost("Add a brand new waiter to restaurant")]
        public Result AddNewWaiterToRestaurant(Guid restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            return Ws.AddNewWaiterToSpecificRestaurant(restaurantID, waiterFirstName, waiterLastName, waiterGender, waiterAge);
        }

        [HttpPost("Add a brand new waiters to restaurant")]
        public Result AddNewWaitersToRestaurant(Guid restaurantId, int waitersNumber)
        {
            return Ws.AddNewDummyListOfWaitersToSpecificRestaurant(restaurantId, waitersNumber);
        }

        [HttpPost("Add a brand new client to restaurant")]
        public Result AddNewClientToRestaurant(Guid restaurantID, string clientFirstName, string clientLastName)
        {
            return Cs.AddNewClientToSpecificRestaurant(restaurantID, clientFirstName, clientLastName);
        }

        [HttpPost("Add a brand new clients to restaurant")]
        public Result AddNewClientsToRestaurant(Guid restaurantId, int clientsNumber)
        {
            return Cs.AddNewDummyListOfClientsToSpecificRestaurant(restaurantId, clientsNumber);
        }

        [HttpGet("Show all Restaurant clients")]
        public List<Client> GetRestaurantClients(Guid restaurantId)
        {
            return Rs.ShowAllSpecificRestaurantClients(restaurantId);
        }

        [HttpGet("Show all Restaurant waiters")]
        public List<Waiter> GetRestaurantWaiters(Guid restaurantId)
        {
            return Rs.ShowAllSpecificRestaurantWaiters(restaurantId);
        }

        [HttpGet("Show all clients by waiter")]
        public List<Client> GetClientsByWaiters(Guid waiterId)
        {
            return Cs.ShowAllClientsBySpecificWaiter(waiterId);
        }

        [HttpPut("Transfer the waiter to another restaurant")]
        public Result TransferWaiter(Guid waiterId, Guid moveIntoRestaurantId)
        {
            return Ws.TransferTheWaiterToAnotherRestaurant(waiterId, moveIntoRestaurantId);
        }

        [HttpPut("Transfer the client to another restaurant")]
        public Result TransferClient(Guid clientId, Guid moveIntoRestaurantId)
        {
            return Cs.TransferTheClientToAnotherRestaurant(clientId, moveIntoRestaurantId);
        }

        [HttpDelete("Delete the restaurant")]
        public Result Delete(Guid restaurantId)
        {
            return Rs.DeleteRestaurant(restaurantId);
        }

        [HttpDelete("Delete the waiter")]
        public Result DeleteWaiter(Guid waiterId)
        {
            return Ws.DeleteTheWaiter(waiterId);
        }

        [HttpDelete("Delete the client")]
        public Result DeleteClient(Guid clientId)
        {
            return Cs.DeleteTheClient(clientId);
        }
    }
}
