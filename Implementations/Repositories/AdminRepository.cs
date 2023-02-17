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
    public class AdminRepository : BaseRepository<Admin> , IAdminRepository
    {
        public AdminRepository(DansnomApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<UserRole>> GetAllAdminsAsync()
        {
            return await _Context.UserRoles
            .Include(x => x.Role)
            .Include(x => x.User)
            .ThenInclude(x => x.Admin)
            .Where(x => x.User.IsDeleted == false && x.Role.Name != "customer")
            .ToListAsync();
        }
        public async Task<Admin> GetAdminAsync(int id)
        {
            return await _Context.Admins
            .Include(admin => admin.User)
            .Where(x => x.User.Id == id && x.IsDeleted == false)
            .SingleOrDefaultAsync();
        }
    }
}