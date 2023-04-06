using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetAllAvailableProductAsync();
        Task<List<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetAllUnavailableProductAsync();
        Task<List<Production>> GetProductsReadyForDelivery();
        Task<List<Product>> GetProductsByCategoryIdAsync(int id);
        Task<Product> GetProductAsync(int id);
    }
    
}