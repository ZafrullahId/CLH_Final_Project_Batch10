using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Enums;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class ProductionRepository : BaseRepository<Production>, IProductionRepository
    {
        public ProductionRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<Production> Create(Production entity)
        {
            await _Context.Productions.AddAsync(entity);
            return entity;
        }
        public async Task<List<Production>> GetProductionsByProductId(int id)
        {
            return await _Context.Productions
            .Include(c => c.Product)
            .Where(x => x.ProductId == id && x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public decimal GetQuantityRemainingByProductId(int id)
        {
            return _Context.Productions
            .Where(x => x.ProductId == id && x.ApprovalStatus == ApprovalStatus.Approved)
            .Sum(x => x.QuantityRemaining);
        }
        public async Task<Production> GetProduction(int id, int productId)
        {
            return await _Context.Productions.SingleOrDefaultAsync(x => x.Id == id && x.ProductId == productId);
        }

        public async Task<List<Production>> GetAllApprovedProduction()
        {
            return await _Context.Productions
            .Where(x => x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<Production>> GetAllRejectedProduction()
        {
            return await _Context.Productions
            .Where(x => x.ApprovalStatus == ApprovalStatus.Approved)
            .ToListAsync();
        }
        public async Task<List<Production>> GetProductionsByDate(string date)
        {
            return await _Context.Productions
            .Include(x => x.Product)
             .Where(x => x.ProductionDate == date && x.ApprovalStatus == ApprovalStatus.Approved)
             .ToListAsync();
        }
        public async Task<List<Production>> GetAllPendingProductionsAsync()
        {
            return await _Context.Productions
            .Include(x => x.Product)
            .Where(x => x.ApprovalStatus == ApprovalStatus.Pending)
            .ToListAsync();
        }

        public async Task<List<Production>> GetAllAprovedProductionsByMonthAsync(int year, int month)
        {
            
            return await _Context.Productions
            .Include(x => x.Product)
            .Where(x => x.ApprovalStatus == ApprovalStatus.Approved && x.CreatedOn.Year == year && x.CreatedOn.Month == month)
            .ToListAsync();
        }
        public async Task<List<Production>> GetAllProductionsAsync()
        {
            return await _Context.Productions
            .Include(c => c.Product)
            .Include(x => x.Admin)
            .ThenInclude(x => x.User)
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync();
        }
    }
}