using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularShop.API.Helpers;
using AngularShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularShop.API.Data
{
    public class ShopRepository : IShopRepository
    {
        private readonly DataContext context;
        public ShopRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(u => u.Id == id);
            return customer;
        }

        public async Task<PagedList<Customer>> GetCustomers(CustomerParams customerParams)
        {
            var customers = context.Customers.OrderByDescending(u => u.Created).AsQueryable();
            customers = customers.Where(u => u.Id != customerParams.CustomerId);
            if (!string.IsNullOrEmpty(customerParams.OrderBy))
            {
                switch (customerParams.OrderBy)
                {
                    case "created":
                        customers = customers.OrderByDescending(u => u.Created);
                        break;
                    default:
                        customers = customers.OrderBy(u => u.Created);
                        break;
                }
            }
            return await PagedList<Customer>.CreatedAsync(customers, customerParams.PageNumber, customerParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<Product>> GetProducts(ProductParams productParams)
        {
            var products = context.Products.AsQueryable();
            switch (productParams.OrderBy)
            {
                case "price":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Price);
                    break;
            }

            return await PagedList<Product>.CreatedAsync(products, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(u => u.Id == id);
            return product;
        }

        public async Task<Purchase> GetPurchase(int id)
        {
            return await context.Purchases.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<PagedList<Purchase>> GetPurchasesForUser()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<Purchase>> GetPurchasesForUser(PurchaseParams purchaseParams)
        {
            var purchases = context.Purchases.Include(p => p.Product)
                .AsQueryable();
            switch (purchaseParams.PurchaseContainer)
            {
                case "Cancelled":
                    purchases = purchases.Where(p => p.IsCancelled == true);
                    break;
                default:
                    purchases = purchases.Where(p => p.IsCancelled == false);
                    break;
            }
            purchases = purchases.Where(u => u.UserId == purchaseParams.UserId);
            purchases = purchases.OrderByDescending(p => p.DateAdded);
            return await PagedList<Purchase>.CreatedAsync(purchases, purchaseParams.PageNumber, purchaseParams.PageSize);

        }
    }
}