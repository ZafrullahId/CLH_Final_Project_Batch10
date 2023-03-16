using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;

namespace Dansnom.Interface.Repositories
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<List<Like>> GetLikesByReviewIdAsync(int reviwId);
    }
}