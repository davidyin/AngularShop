using System;
using System.Collections.Generic;

namespace AngularShop.API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }



    }
}