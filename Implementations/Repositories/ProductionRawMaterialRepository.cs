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
    public class ProductionRawMaterialRepository : BaseRepository<ProductionRawMaterial> , IProductionRawMaterialRepository
    {
        public ProductionRawMaterialRepository(DansnomApplicationContext context)
        {
            _Context = context;
        }
         public async Task<ProductionRawMaterial> Create(ProductionRawMaterial entity)
        {
            await _Context.ProductionRawMaterials.AddAsync(entity);
            return entity;
        }
         public async Task<List<ProductionRawMaterial>> GetAllAprovedProductionsByYearAsync(int year)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Year == year && x.Production.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
         public async Task<List<ProductionRawMaterial>> GetAllApprovedYearlyProduction(int year,int id)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Year == year && x.Production.Product.Id == id && x.Production.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
         public async Task<List<ProductionRawMaterial>> GetAllApprovedMonthlyProduction(int month,int id)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Month == month  && x.Production.Product.Id == id && x.Production.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetAllRejectedProductionsByYearAsync(int year)
        {
             return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Year == year && x.Production.ApprovalStatus == ApprovalStatus.Rejected)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetAllAprovedProductionsByMonthAsync(int month)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Month == month && x.Production.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetAllRejectedProductionsByMonthAsync(int month)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.CreatedOn.Month == month && x.Production.ApprovalStatus == ApprovalStatus.Rejected)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetAllPendingProductionsAsync()
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.ApprovalStatus == ApprovalStatus.Pending)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetProductionsByDate(string date)
        {
           return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.ProductionDate == date)
            .ToListAsync();
        }
         public async Task<List<ProductionRawMaterial>> GetProductionsByProductId(int id)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .ThenInclude(c => c.Product)
            .Where(x => x.Production.ProductId == id && x.RawMaterial.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<ProductionRawMaterial>> GetProductionsById(int id)
        {
            return await _Context.ProductionRawMaterials
            .Include(x => x.RawMaterial)
            .Include(c => c.Production)
            .Where(x => x.ProductionId == id)
            .ToListAsync();
        }
    }
}