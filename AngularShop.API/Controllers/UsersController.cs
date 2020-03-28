
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AngularShop.API.Data;
using AngularShop.API.Dtos;
using AngularShop.API.Helpers;
using AngularShop.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IShopRepository repository;
        private readonly IMapper mapper;
        public UsersController(IShopRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] CustomerParams customerParms)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var customerFromRepo = await repository.GetCustomer(currentUserId);
            customerParms.CustomerId = currentUserId;

            var customers = await repository.GetCustomers(customerParms);
            var customersToReturn = mapper.Map<IEnumerable<CustomerDto>>(customers);
            Response.AddPagingation(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);
            return Ok(customersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repository.GetCustomer(id);
            var userToReturn = mapper.Map<CustomerDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, CustomerDto userForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await repository.GetCustomer(id);
            mapper.Map(userForUpdate, userFromRepo);
            if (await repository.SaveAll())
                return NoContent();
            throw new System.Exception($"Updating user {id} failed on save");
        }






    }
}