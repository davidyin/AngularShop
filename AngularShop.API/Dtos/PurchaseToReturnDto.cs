using System;
using AngularShop.API.Models;

namespace AngularShop.API.Dtos
{
    public class PurchaseToReturnDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsCancelled { get; set; }
    }
}