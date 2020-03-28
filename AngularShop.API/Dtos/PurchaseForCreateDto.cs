using System;

namespace AngularShop.API.Dtos
{
    public class PurchaseForCreateDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsCancelled { get; set; }
    }
}