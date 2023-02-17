using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface IRawMaterialRepository : IBaseRepository<RawMaterial>
    {
         Task<List<RawMaterial>> GetAllAprovedRawMaterialForTheMonthAsync(int month,int year);
        Task<List<RawMaterial>> GetAllAprovedRawMaterialForTheYearAsync(int year);
        Task<List<RawMaterial>> GetAllRawMaterialAsync();
        Task<List<RawMaterial>> GetAllRejectedRawMaterialForTheMonthAsync(int month);
        Task<List<RawMaterial>> GetAllRejectedRawMaterialForTheYearAsync(int year);
        Task<decimal> GetSumOfAprovedRawMaterialForTheMonthAsync(int  month, int year);
        Task<decimal> GetSumOfAprovedRawMaterialForTheMonthAsync();
        Task<decimal> GetSumOfAprovedRawMaterialForTheYearAsync(int year);
        Task<decimal> GetSumOfAprovedRawMaterialForTheYearAsync();
        Task<List<RawMaterial>> GetAllApprovedRawMaterialsAsync();
        Task<List<RawMaterial>> GetAllPendingRawMaterialsAsync();
    }
    
}