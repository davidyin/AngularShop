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
using System;

namespace AngularShop.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IShopRepository repository;
        private readonly IMapper mapper;

        public PurchasesController(IShopRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPurchase")]
        public async Task<IActionResult> GetPurchase(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var purchaseFromRepo = await repository.GetPurchase(id);
            if (purchaseFromRepo == null)
                return NotFound();

            return Ok(purchaseFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchasesForUser(int userId, [FromQuery]PurchaseParams purchaseParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            purchaseParams.UserId = userId;

            var purchasesFromRepo = await repository.GetPurchasesForUser(purchaseParams);
            var purchases = mapper.Map<IEnumerable<PurchaseToReturnDto>>(purchasesFromRepo);
            Response.AddPagingation(purchasesFromRepo.CurrentPage, purchasesFromRepo.PageSize,
            purchasesFromRepo.TotalCount, purchasesFromRepo.TotalPages);
            return Ok(purchases);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatePurchase(int userId, PurchaseForCreateDto purchaseForCreateDto)
        {
            var sender = await repository.GetCustomer(userId);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            purchaseForCreateDto.UserId = userId;
            var product = await repository.GetProduct(purchaseForCreateDto.ProductId);
            if (product == null)
                return BadRequest("Could not find product");

            purchaseForCreateDto.DateAdded = DateTime.Now;
            purchaseForCreateDto.IsCancelled = false;

            var purchase = mapper.Map<Purchase>(purchaseForCreateDto);
            repository.Add(purchase);

            if (await repository.SaveAll())
            {
                var purchaseToReturn = mapper.Map<PurchaseToReturnDto>(purchase);
                return CreatedAtRoute("GetPurchase", new { userId, id = purchase.Id }, purchaseToReturn);
            }

            throw new System.Exception("Create purchase failed on save");

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CancelPurchase(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var purchaseFromRepo = await repository.GetPurchase(id);
            if (purchaseFromRepo.UserId == userId)
            {
                purchaseFromRepo.IsCancelled = true;
            }
            if (await repository.SaveAll())
                return NoContent();

            throw new System.Exception("Error deleting the messge");
        }


    }
}