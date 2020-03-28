
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
    public class ProductsController : ControllerBase
    {
        private readonly IShopRepository repository;
        private readonly IMapper mapper;

        public ProductsController(IShopRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
        {
            var productsFromRepo = await repository.GetProducts(productParams);

            Response.AddPagingation(productsFromRepo.CurrentPage, productsFromRepo.PageSize, productsFromRepo.TotalCount, productsFromRepo.TotalPages);
            return Ok(productsFromRepo);
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await repository.GetProduct(id);
            var productToReturn = mapper.Map<ProductToReturnDto>(product);
            return Ok(productToReturn);
        }
    }



}