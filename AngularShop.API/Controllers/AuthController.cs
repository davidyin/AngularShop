using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AngularShop.API.Data;
using AngularShop.API.Dtos;
using AngularShop.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

namespace AngularShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;

        private readonly IConfiguration config;
        private readonly IMapper mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            this.config = config;
            this.mapper = mapper;
            this.repo = repo;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerForRegisterDto customerForRegisterDto)
        {

            var email = customerForRegisterDto.Email.ToLower();

            if (await repo.CustomerExists(email))
                return BadRequest("Username already exists");

            var customerToCreate = mapper.Map<Customer>(customerForRegisterDto);

            var createdCustomer = await repo.Register(customerToCreate, customerForRegisterDto.Password);
            var customerToReturn = mapper.Map<CustomerDto>(createdCustomer);

            return CreatedAtRoute("GetCustomer", new { controller = "Customers", id = createdCustomer.Id }, customerToReturn);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerForLoginDto customerForLoginDto)
        {
            var customerFromRepo = await repo.Login(customerForLoginDto.Email.ToLower(), customerForLoginDto.Password);
            if (customerFromRepo == null)
                return Unauthorized();

            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, customerFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, customerFromRepo.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),   // 24 hours
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = mapper.Map<CustomerDto>(customerFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });

        }



    }
}