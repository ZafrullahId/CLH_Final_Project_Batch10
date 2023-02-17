using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetAllOrderAsync();
        Task<List<Order>> GetAllDeleveredOrderAsync();
        Task<List<Order>> GetAllUnDeleveredOrderAsync();
    }
    
}