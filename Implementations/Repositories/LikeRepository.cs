using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository 
    {
        public LikeRepository(DansnomApplicationContext context)
        {
            _Context = context;   
        }
        public async Task<List<Like>> GetLikesByReviewIdAsync(int reviwId) => await _Context.Likes
        .Include(x => x.User)
        .Where(x => x.ReviewId == reviwId && x.IsDeleted == false).ToListAsync();

    }
}