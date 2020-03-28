using System.Collections.Generic;
using System.Linq;
using AngularShop.API.Models;
using Newtonsoft.Json;
using System.IO;
namespace AngularShop.API.Data
{
    public class Seed
    {
        public static void SeedCustomers(DataContext context)
        {
            if (!context.Customers.Any())
            {
                var customerData = File.ReadAllText("Data/CustomerSeedData.json");

                var customers = JsonConvert.DeserializeObject<List<Customer>>(customerData);
                foreach (var customer in customers)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    customer.PasswordHash = passwordHash;
                    customer.PasswordSalt = passwordSalt;
                    customer.Email = customer.Email.ToLower();
                    context.Customers.Add(customer);
                }


                var productData = File.ReadAllText("Data/Product.json");
                var products = JsonConvert.DeserializeObject<List<Product>>(productData);
                foreach (var product in products)
                {
                    context.Products.Add(product);
                }


                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
    }

}