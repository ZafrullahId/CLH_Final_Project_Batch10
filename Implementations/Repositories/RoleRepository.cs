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
    public class RoleRepository : BaseRepository<Role> , IRoleRepository
    {
        public RoleRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
         public async Task<Role> GetRoleByUserId(int id)
        {
            var role = await _Context.UserRoles
            .Include(c => c.User)
            .Include(x => x.Role).SingleOrDefaultAsync(x => x.UserId == id);
            return role.Role;
        }
        public async Task<List<Role>> GetRoleAsync()
        {
            return await _Context.Roles
            .Where(x => x.IsDeleted == false).ToListAsync();
        }
    } 
}