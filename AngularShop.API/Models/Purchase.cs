using System;

namespace AngularShop.API.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime DateAdded { get; set; }

        public bool IsCancelled { get; set; }
    }
}