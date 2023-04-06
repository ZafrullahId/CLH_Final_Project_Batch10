using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class ProductRepository : BaseRepository<Product> , IProductRepository
    {
        public ProductRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }

        public async Task<List<Product>> GetAllAvailableProductAsync()
        {
            return await _Context.Products.Where(x => x.isAvailable == true && x.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllUnavailableProductAsync()
        {
            return await _Context.Products.Where(x => x.isAvailable == false && x.IsDeleted == false).ToListAsync();
        }
         public async Task<List<Product>> GetAllProductsAsync()
         {
            return await _Context.Products.Where(x => x.IsDeleted == false).ToListAsync();
         }
         public async Task<List<Production>> GetProductsReadyForDelivery()
         {
            return await _Context.Productions
            .Include(c => c.Product)
            .Where(x => x.QuantityRemaining != 0.0m && x.Product.IsDeleted == false).ToListAsync();
         }
         public async Task<List<Product>> GetProductsByCategoryIdAsync(int id)
         {
            return await _Context.Products
            .Where(x => x.CategoryId == id && x.IsDeleted == false).ToListAsync();
         }
         public async Task<Product> GetProductAsync(int id)
         {
            return await _Context.Products
            .Include(x => x.Category)
            .Where(x => x.Id == id).SingleOrDefaultAsync();
         }
    } 
}