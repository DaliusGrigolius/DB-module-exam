using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Waiter> Waiters { get; set; }
        [Required]
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public Client(string firstName, string lastName, Guid restaurantId)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Waiters = new List<Waiter>();
            RestaurantId = restaurantId;
        }

        public Client(string firstName, string lastName, string phoneNumber, string email, Guid restaurantId)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Waiters = new List<Waiter>();
            RestaurantId = restaurantId;
        }
    }
}
