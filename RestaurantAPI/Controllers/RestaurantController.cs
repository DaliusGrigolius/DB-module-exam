using Business;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        readonly RestaurantServices rs = new();
        readonly WaiterServices ws = new();
        readonly ClientServices cs = new();

        [HttpPost("Create a brand new restaurant with waiters and clients in it")]
        public Result CreateNewFilledRestaurant(string name, string address, string email, string phone, int waitersNumber, int clientsNumber)
        {
            return rs.CreateRestaurantWithNewWaitersAndClients(name, address, email, phone, waitersNumber, clientsNumber);
        }

        [HttpPost("Create a brand new empty restaurant")]
        public Result CreateNewEmptyRestaurant(string name, string address, string email, string phone)
        {
            return rs.CreateEmptyRestaurant(name, address, email, phone);
        }

        [HttpPost("Add a brand new waiter to restaurant")]
        public Result AddNewWaiterToRestaurant(string restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge)
        {
            return ws.AddNewWaiterToSpecificRestaurant(restaurantID, waiterFirstName, waiterLastName, waiterGender, waiterAge);
        }

        [HttpPost("Add a brand new waiters to restaurant")]
        public Result AddNewWaitersToRestaurant(string restaurantId, int waitersNumber)
        {
            return ws.AddNewDummyListOfWaitersToSpecificRestaurant(restaurantId, waitersNumber);
        }

        [HttpPost("Add a brand new client to restaurant")]
        public Result AddNewClientToRestaurant(string restaurantID, string clientFirstName, string clientLastName)
        {
            return cs.AddNewClientToSpecificRestaurant(restaurantID, clientFirstName, clientLastName);
        }

        [HttpPost("Add a brand new clients to restaurant")]
        public Result AddNewClientsToRestaurant(string restaurantId, int clientsNumber)
        {
            return cs.AddNewDummyListOfClientsToSpecificRestaurant(restaurantId, clientsNumber);
        }

        [HttpGet("Show all Restaurant clients")]
        public List<Client> GetRestaurantClients(string restaurantId)
        {
            return rs.ShowAllSpecificRestaurantClients(restaurantId);
        }

        [HttpGet("Show all Restaurant waiters")]
        public List<Waiter> GetRestaurantWaiters(string restaurantId)
        {
            return rs.ShowAllSpecificRestaurantWaiters(restaurantId);
        }

        [HttpGet("Show all clients by waiter")]
        public List<Client> GetClientsByWaiters(string waiterId)
        {
            return cs.ShowAllClientsBySpecificWaiter(waiterId);
        }

        [HttpPut("Transfer the waiter to another restaurant")]
        public Result TransferWaiter(string waiterId, string moveIntoRestaurantId)
        {
            return ws.TransferTheWaiterToAnotherRestaurant(waiterId, moveIntoRestaurantId);
        }

        [HttpPut("Transfer the client to another restaurant")]
        public Result TransferClient(string clientId, string moveIntoRestaurantId)
        {
            return cs.TransferTheClientToAnotherRestaurant(clientId, moveIntoRestaurantId);
        }

        [HttpDelete("Delete the restaurant")]
        public Result Delete(string restaurantId)
        {
            return rs.DeleteRestaurant(restaurantId);
        }

        [HttpDelete("Delete the waiter")]
        public Result DeleteWaiter(string waiterId)
        {
            return ws.DeleteTheWaiter(waiterId);
        }

        [HttpDelete("Delete the client")]
        public Result DeleteClient(string clientId)
        {
            return cs.DeleteTheClient(clientId);
        }
    }
}
