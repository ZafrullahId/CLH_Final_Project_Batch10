using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class ProductOrdersRepository : BaseRepository<ProductOrders> , IProductOrdersRepository
    {
        public ProductOrdersRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<ProductOrders>> GetAllOrders()
        {
            return await _Context.ProductOrders
            .Include(c => c.Product)
            .Include(c => c.Order)
            .Include(c => c.Order.Customer)
            .Include(c => c.Order.Address)
            .Where(c => c.Order.isDelivered == false)
            .ToListAsync();
        }
        public async Task<ProductOrders> GetOrderById(int id)
        {
            return await _Context.ProductOrders
            .Include(c => c.Product)
            .Include(c => c.Order)
            .Include(c => c.Order.Customer)
            .Include(c => c.Order.Address)
            .Where(c => c.OrderId == id)
            .SingleOrDefaultAsync();
        }
    } 
}