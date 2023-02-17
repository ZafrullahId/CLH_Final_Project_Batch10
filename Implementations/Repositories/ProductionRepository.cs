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
    public class ProductionRepository : BaseRepository<Production> , IProductionRepository
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
            .Where(x => x.ProductId == id)
            .ToListAsync();
        }
        public decimal GetQuantityRemainingByProductId(int id)
        {
            return _Context.Productions
            .Where(x => x.ProductId == id && x.ApprovalStatus == ApprovalStatus.Approved)
            .Sum(x => x.QuantityRemaining);
        }
        public async Task<Production> GetProduction(int id,int productId)
        {
            return await _Context.Productions.SingleOrDefaultAsync(x => x.Id == id && x.ProductId == productId);
        }
         
    } 
}