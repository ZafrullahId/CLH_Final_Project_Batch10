using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class SalesRepository : BaseRepository<Sales> , ISalesRepository
    {
        public SalesRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<Sales>> GetSaleByCustomerIdAsync(int id)
        {
            return await _Context.Sales
            .Include(c => c.Order)
            .Include(c => c.Order.Customer)
            .Where(c => c.Order.CustomerId == id)
            .ToListAsync();
        }
    //     public async Task<List<Sales>> GetSalesByCustomerIdAndDateAsync(int id,DateTime dateOrded)
    //     {
    //         return await _Context.Sales
    //         .Include(c => c.Order)
    //         .Include(x => x.Product)
    //         .Where(c => c.Order.CustomerId == id && c.Order.CreatedOn == dateOrded)
    //         .ToListAsync();
    //     }
    //     public async Task<List<Sales>> GetSalesForTheMonthAsync(int id,int month,int year)
    //     {
    //         return await _Context.Sales
    //         .Include(x => x.Order)
    //         .ThenInclude(x => x.Customer)
    //         .ThenInclude(x => x.User)
    //         .Where(c => c.ProductId == id && c.CreatedOn.Month == month && c.CreatedOn.Year == year)
    //         .ToListAsync();
            
    //     }
        
    //    public async Task<List<Sales>> GetSalesForTheYearAsync(int id,int year)
    //     {
    //          return await _Context.Sales
    //         .Include(x => x.Order.Address)
    //         .Include(x => x.Order)
    //         .ThenInclude(x => x.Customer)
    //         .ThenInclude(x => x.User)
    //         .Where(c => c.ProductId == id && c.CreatedOn.Year == year)
    //         .ToListAsync();
    //     }
       public async Task<List<Sales>> GetAllSales()
        {
             return await _Context.Sales
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .ToListAsync();
        }
       public async Task<List<Sales>> GetThisYearSales()
        {
             return await _Context.Sales
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Where(x => x.CreatedOn.Year == DateTime.Now.Year)
            .ToListAsync();
        }
       public async Task<List<Sales>> GetThisMonthSales()
        {
             return await _Context.Sales
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Where(x => x.CreatedOn.Year == DateTime.Now.Year && x.CreatedOn.Month == DateTime.Now.Month)
            .ToListAsync();
        }
         public async Task<decimal> GetTotalMonthlySalesAsync()
        {
            return await _Context.Sales
            .Where(x => x.CreatedOn.Month == DateTime.Now.Month)
            .SumAsync(x => x.AmountPaid);
        }
         public async Task<decimal> GetTotalYearlySalesAsync()
        {
            return await _Context.Sales
            .Where(x => x.CreatedOn.Year == DateTime.Now.Year)
            .SumAsync(x => x.AmountPaid);
        }
        public async Task<decimal> GetTotalMonthlySalesAsync(int month,int year)
        {
            return await _Context.Sales
            .Where(x => x.CreatedOn.Month == month && x.CreatedOn.Year == year)
            .SumAsync(x => x.AmountPaid);
        }
        public async Task<decimal> GetTotalYearlySalesAsync(int year)
        {
            return await _Context.Sales
            .Where(x => x.CreatedOn.Year == year)
            .SumAsync(x => x.AmountPaid);
        }
    } 
}