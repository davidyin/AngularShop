using System;
using System.Threading.Tasks;
using AngularShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularShop.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<Customer> Login(string email, string password)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.Email == email);
            if (customer == null) return null;
            if (!VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
                return null;
            return customer;
        }

        public async Task<Customer> Register(Customer customer, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> CustomerExists(string email)
        {
            if (await context.Customers.AnyAsync(x => x.Email == email))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

    }
}