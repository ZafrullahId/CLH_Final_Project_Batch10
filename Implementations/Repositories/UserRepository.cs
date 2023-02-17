using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<UserRole> LoginAsync(string email, string password)
        {
            return await _Context.UserRoles
            .Where(c => c.User.Email == email && c.User.Password == password && c.User.IsDeleted == false)
            .Include(c => c.Role)
            .Include(x => x.User)
            .SingleOrDefaultAsync();
        }
        public async Task<List<UserRole>> GetUserByRoleAsync(string role)
        {
            return await _Context.UserRoles
            .Where(x => x.Role.Name == role)
            .Include(c => c.User).Include(c => c.Role)
            .ToListAsync();
        }
    } 
}