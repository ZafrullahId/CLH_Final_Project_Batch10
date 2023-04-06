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
        public async Task<List<ProductOrders>> GetOrdersByIdAsync(int id)
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.OrderId == id)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetOrderByCustomerIdAsync(int id)
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.Order.CustomerId == id)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetAllOrderAsync()
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetAllDeleveredOrderByProductIdForTheMonthAsync(int id,int month,int year)
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.Order.isDelivered == true && x.ProductId == id && x.Order.LastModifiedOn.Month == month && x.Order.LastModifiedOn.Year == year)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetAllDeleveredOrderByProductIdForTheYearAsync(int id,int year)
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.Order.isDelivered == true && x.ProductId == id && x.Order.LastModifiedOn.Year == year)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetAllDeleveredOrderAsync()
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.Order.isDelivered == true)
            .ToListAsync();
        }
        public async Task<List<ProductOrders>> GetAllUnDeleveredOrderAsync()
        {
            return await _Context.ProductOrders
            .Include(x => x.Product)
            .Include(x => x.Order.Address)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.Order.isDelivered == false)
            .ToListAsync();
        }
    } 
}