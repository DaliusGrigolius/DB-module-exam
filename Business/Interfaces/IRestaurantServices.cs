using Repository.Entities;
using System;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IRestaurantServices
    {
        Result CreateEmptyRestaurant(string restaurantName, string restaurantAddress, string restaurantEmail, string restaurantPhone);
        Result CreateRestaurantWithNewWaitersAndClients(string name, string address, string email, string phone, int waitersNumber, int clientsNumber);
        Result DeleteRestaurant(Guid restaurantId);
        List<Client> ShowAllSpecificRestaurantClients(Guid restaurantId);
        List<Waiter> ShowAllSpecificRestaurantWaiters(Guid restaurantId);
    }
}