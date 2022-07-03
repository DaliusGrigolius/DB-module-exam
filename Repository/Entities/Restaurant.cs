using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public virtual List<Client> Clients { get; set; }
        public virtual List<Waiter> Waiters { get; set; }

        public Restaurant(string name, string address, string email, string phoneNumber)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Clients = new List<Client>();
            Waiters = new List<Waiter>();
        }

        public Restaurant(string name, string address, string email, string phoneNumber, List<Client> clients, List<Waiter> waiters)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            Clients = clients;
            Waiters = waiters;
        }

        public Restaurant()
        {

        }
    }
}
