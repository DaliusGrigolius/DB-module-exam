﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class Waiter
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }
        public List<Client> Clients { get; set; }
        [ForeignKey("Restaurant")]
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public Waiter(string firstName, string lastName, string gender, int age, Guid restaurantId)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Age = age;
            Clients = new List<Client>();
            RestaurantId = restaurantId;
        }
    }
}
