using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class OrderRepository : BaseRepository<Order> , IOrderRepository
    {
        public OrderRepository(DansnomApplicationContext context)
        {
            _Context = context;
        }
        public async Task<List<Order>> GetAllDeleveredOrdersAsync()
        {
            return await _Context.Orders
            .Where(x => x.isDelivered == true)
            .ToListAsync();
        }
        public async Task<List<Order>> GetAllUnDeleveredOrdersAsync()
        {
            return await _Context.Orders
            .Where(x => x.isDelivered == false)
            .ToListAsync();
        }
        public async Task<List<Order>> GetOrderByCustomerId(int id)
        {
            return await _Context.Orders
            .Include(x => x.Customer)
            .Include(x => x.Address)
            .Where(x => x.CustomerId == id).ToListAsync();
        }
    } 
}