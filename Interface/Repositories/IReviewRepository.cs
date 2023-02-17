using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public  interface IReviewRepository : IBaseRepository<Review>
    {
        Task<List<Review>> GetReviewsByCustomerIdAsync(int id);
        Task<List<Review>> GetAllReviewsAsync();
        Task<List<Review>> GetAllUnseenReviewsAsync();
        Task<Review> GetReviewById(int id);
    }
    
}