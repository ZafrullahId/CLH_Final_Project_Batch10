using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IProductionRepository : IBaseRepository<Production>
    {
        Task<Production> GetProduction(int id, int productId);
        Task<List<Production>> GetProductionsByProductId(int id);
        decimal GetQuantityRemainingByProductId(int id);
        Task<List<Production>> GetAllApprovedProduction();
        Task<List<Production>> GetProductionsByDate(string date);
        Task<List<Production>> GetAllPendingProductionsAsync();
        Task<List<Production>> GetAllAprovedProductionsByMonthAsync(int year, int month);
        Task<List<Production>> GetAllRejectedProduction();
    }

}