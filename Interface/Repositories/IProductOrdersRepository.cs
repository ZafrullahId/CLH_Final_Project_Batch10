using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IProductOrdersRepository : IBaseRepository<ProductOrders>
    {
        Task<List<ProductOrders>> GetAllOrderAsync();
        Task<List<ProductOrders>> GetOrdersByIdAsync(int id);
        Task<List<ProductOrders>> GetAllDeleveredOrderAsync();
        Task<List<ProductOrders>> GetAllUnDeleveredOrderAsync();
        Task<List<ProductOrders>> GetOrderByCustomerIdAsync(int id);
        Task<List<ProductOrders>> GetAllDeleveredOrderByProductIdForTheYearAsync(int id, int year);
        Task<List<ProductOrders>> GetAllDeleveredOrderByProductIdForTheMonthAsync(int id, int month, int year);
    }

}