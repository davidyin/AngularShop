using System.Linq;
using AutoMapper;
using AngularShop.API.Dtos;
using AngularShop.API.Models;

namespace AngularShop.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerForRegisterDto, Customer>();
            CreateMap<ProductToReturnDto, Product>();
            CreateMap<Product, ProductToReturnDto>();

            CreateMap<PurchaseForCreateDto, Purchase>().ReverseMap();

            CreateMap<PurchaseToReturnDto, Purchase>();
            CreateMap<Purchase, PurchaseToReturnDto>();
        }

    }
}