using Repository.Entities;
using System;

namespace Business.Interfaces
{
    public interface IWaiterServices
    {
        Result AddNewDummyListOfWaitersToSpecificRestaurant(Guid restaurantId, int waitersNumber);
        Result AddNewWaiterToSpecificRestaurant(Guid restaurantID, string waiterFirstName, string waiterLastName, string waiterGender, int waiterAge);
        Result DeleteTheWaiter(Guid waiterId);
        Result TransferTheWaiterToAnotherRestaurant(Guid waiterId, Guid moveIntoRestaurantId);
    }
}