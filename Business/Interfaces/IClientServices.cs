using Repository.Entities;
using System;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IClientServices
    {
        Result AddNewClientToSpecificRestaurant(Guid restaurantID, string clientFirstName, string clientLastName);
        Result AddNewDummyListOfClientsToSpecificRestaurant(Guid restaurantId, int clientsNumber);
        Result DeleteTheClient(Guid clientId);
        List<Client> ShowAllClientsBySpecificWaiter(Guid waiterId);
        Result TransferTheClientToAnotherRestaurant(Guid clientId, Guid moveIntoRestaurantId);
    }
}