using System.Collections.Generic;
using System.Threading.Tasks;
using AngularShop.API.Models;
using AngularShop.API.Helpers;
namespace AngularShop.API.Data
{
    public interface IShopRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<Customer>> GetCustomers(CustomerParams customerParams);
        Task<Customer> GetCustomer(int id);
        Task<PagedList<Product>> GetProducts(ProductParams productParams);
        Task<Product> GetProduct(int id);

        Task<Purchase> GetPurchase(int id);
        Task<PagedList<Purchase>> GetPurchasesForUser(PurchaseParams purchaseParams);

    }
}