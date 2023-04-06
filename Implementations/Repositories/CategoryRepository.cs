using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;

namespace Dansnom.Implementations.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DansnomApplicationContext context)
        {
            _Context = context;
        }
        
    }
}