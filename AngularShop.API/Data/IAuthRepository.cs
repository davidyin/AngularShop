using System.Threading.Tasks;
using AngularShop.API.Models;

namespace AngularShop.API.Data
{
    public interface IAuthRepository
    {
        Task<Customer> Register(Customer customer, string password);
        Task<Customer> Login(string email, string password);
        Task<bool> CustomerExists(string email);
    }
}