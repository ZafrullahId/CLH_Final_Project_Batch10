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
        Task<Production> Create(Production entity);
        Task<List<Production>> GetAllApprovedProduction();
    }
    
}