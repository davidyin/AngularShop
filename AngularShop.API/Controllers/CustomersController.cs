
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
    public class CustomersController : ControllerBase
    {
        private readonly IShopRepository repository;
        private readonly IMapper mapper;
        public CustomersController(IShopRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomerParams customerParams)
        {
            var currentCustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var customerFromRepo = await repository.GetCustomer(currentCustomerId);
            customerParams.CustomerId = currentCustomerId;

            var customers = await repository.GetCustomers(customerParams);
            var customersToReturn = mapper.Map<IEnumerable<CustomerDto>>(customers);
            Response.AddPagingation(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);
            return Ok(customersToReturn);
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await repository.GetCustomer(id);
            var customerToReturn = mapper.Map<CustomerDto>(customer);
            return Ok(customerToReturn);
        }



    }
}