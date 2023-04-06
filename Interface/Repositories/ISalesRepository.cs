using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface ISalesRepository : IBaseRepository<Sales>
    {
        Task<List<Sales>> GetAllSales();
        Task<List<Sales>> GetSaleByCustomerIdAsync(int id);
        Task<List<Sales>> GetThisMonthSales();
        Task<List<Sales>> GetThisYearSales();
        Task<decimal> GetTotalMonthlySalesAsync();
        Task<decimal> GetTotalMonthlySalesAsync(int month, int year);
        Task<decimal> GetTotalYearlySalesAsync();
        Task<decimal> GetTotalYearlySalesAsync(int year);
    }

}