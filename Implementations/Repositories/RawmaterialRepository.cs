using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using Dansnom.Enums;


namespace Dansnom.Implementations.Repositories
{
    public class RawMaterialRepository : BaseRepository<RawMaterial> , IRawMaterialRepository
    {
        public RawMaterialRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<RawMaterial>> GetAllRawMaterialAsync()
        {
            return await _Context.RawMaterials.OrderByDescending(x => x.CreatedOn)
            .ToListAsync();
        }
        public async Task<decimal> GetSumOfAprovedRawMaterialForTheYearAsync(int year)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Approved)
            .SumAsync(x => x.Cost);
        }
        public async Task<decimal> GetSumOfAprovedRawMaterialForTheYearAsync()
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Year == DateTime.Now.Year && x.ApprovalStatus == ApprovalStatus.Approved)
            .SumAsync(x => x.Cost);
        }
        public async Task<decimal> GetSumOfAprovedRawMaterialForTheMonthAsync(int  month, int year)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Month == month && x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Approved)
            .SumAsync(x => x.Cost);
        }
        public async Task<decimal> GetSumOfAprovedRawMaterialForTheMonthAsync()
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Month == DateTime.Now.Month && x.ApprovalStatus == ApprovalStatus.Approved)
            .SumAsync(x => x.Cost);
        }
        public async Task<List<RawMaterial>> GetAllAprovedRawMaterialForTheYearAsync(int year)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<RawMaterial>> GetAllRejectedRawMaterialForTheYearAsync(int year)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Rejected)
            .ToListAsync();
        }
        public async Task<List<RawMaterial>> GetAllAprovedRawMaterialForTheMonthAsync(int month,int year)
        {
             return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Month == month && x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<RawMaterial>> GetAllRejectedRawMaterialForTheMonthAsync(int month)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Month == month && x.ApprovalStatus == ApprovalStatus.Rejected)
            .ToListAsync();
        }
         public async Task<List<RawMaterial>> GetAllPendingRawMaterialForTheYearAsync(int year)
        {
            return await _Context.RawMaterials
            .Where(x => x.CreatedOn.Year == year && x.ApprovalStatus == ApprovalStatus.Pending)
            .ToListAsync();
        }
        public async Task<List<RawMaterial>> GetAllPendingRawMaterialsAsync()
        {
            return await _Context.RawMaterials
            .Where(x => x.ApprovalStatus == ApprovalStatus.Pending)
            .ToListAsync();
        }
        public async Task<List<RawMaterial>> GetAllApprovedRawMaterialsAsync()
        {
            return await _Context.RawMaterials
            .Where(x => x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        
    } 
}