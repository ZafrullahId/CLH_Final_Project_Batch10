using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetAllDeleveredOrdersAsync();
        Task<List<Order>> GetOrderByCustomerId(int id);
        Task<List<Order>> GetAllUnDeleveredOrdersAsync();
    }
    
}