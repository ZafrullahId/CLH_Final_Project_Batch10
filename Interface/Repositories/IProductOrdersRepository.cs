using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IProductOrdersRepository : IBaseRepository<ProductOrders>
    {
        Task<List<ProductOrders>> GetAllOrders();
        Task<ProductOrders> GetOrderById(int id);
    }
    
}