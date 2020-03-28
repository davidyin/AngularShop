using System;
using System.ComponentModel.DataAnnotations;
namespace AngularShop.API.Dtos
{
    public class CustomerForRegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime Created { get; set; }


    }
}