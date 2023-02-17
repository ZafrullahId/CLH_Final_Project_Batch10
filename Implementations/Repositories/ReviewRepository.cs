using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class ReviewRepository : BaseRepository<Review> , IReviewRepository
    {
        public ReviewRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<Review>> GetReviewsByCustomerIdAsync(int id)
        {
            return await _Context.Reviews
            .Where(x => x.CustomerId == id)
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.IsDeleted == false)
            .ToListAsync();
        }
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _Context.Reviews
            .Include(c => c.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync();
        }
        public async Task<List<Review>> GetAllUnseenReviewsAsync()
        {
            return await _Context.Reviews
            .Include(c => c.Customer)
            .ThenInclude(x => x.User)
            .Where(x => x.Seen == false && x.IsDeleted == false)
            .ToListAsync();
            
        }
        public async Task<Review> GetReviewById(int id)
        {
            return await _Context.Reviews
            .Include(x => x.Customer)
            .ThenInclude(x => x.User)
            .Where(c => c.Id == id)
            .SingleAsync();
        }
    } 
}