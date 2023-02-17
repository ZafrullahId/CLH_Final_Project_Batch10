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
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _Context.Orders
            .Include(x => x.Address)
            .Include(x => x.Customer)
            .Include(x => x.Customer.User)
            .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }
        public async Task<List<Order>> GetAllOrderAsync()
        {
            return await _Context.Orders
            .Include(x => x.Address)
            .Include(x => x.Customer)
            .Include(x => x.Customer.User)
            .ToListAsync();
        }
        public async Task<List<Order>> GetAllDeleveredOrderAsync()
        {
            return await _Context.Orders
            .Include(x => x.Address)
            .Include(x => x.Customer)
            .Include(x => x.Customer.User)
            .Where(x => x.isDelivered == true)
            .ToListAsync();
        }
        public async Task<List<Order>> GetAllUnDeleveredOrderAsync()
        {
            return await _Context.Orders
            .Include(x => x.Address)
            .Include(x => x.Customer)
            .Include(x => x.Customer.User)
            .Where(x => x.isDelivered == false)
            .ToListAsync();
        }
    } 
}