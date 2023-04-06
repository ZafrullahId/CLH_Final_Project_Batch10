using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IProductionRawMaterialRepository : IBaseRepository<ProductionRawMaterial>
    {
       Task<List<ProductionRawMaterial>> GetAllAprovedProductionsByMonthAsync(int month);
       
        Task<List<ProductionRawMaterial>> GetAllAprovedProductionsByYearAsync(int year);
        Task<List<ProductionRawMaterial>> GetAllPendingProductionsAsync();
        Task<List<ProductionRawMaterial>> GetAllApprovedMonthlyProduction(int month,int id);
        Task<List<ProductionRawMaterial>> GetAllRejectedProductionsByMonthAsync(int month);
        Task<List<ProductionRawMaterial>> GetAllRejectedProductionsByYearAsync(int year);
        Task<List<ProductionRawMaterial>> GetAllApprovedYearlyProduction(int year,int id);
        Task<List<ProductionRawMaterial>> GetProductionsByProductId(int id);
        Task<ProductionRawMaterial> Create(ProductionRawMaterial entity);
        Task<List<ProductionRawMaterial>> GetProductionsById(int id);
        Task<List<ProductionRawMaterial>> GetProductionsByRawMaterialIdAsync(int id);
    }
}