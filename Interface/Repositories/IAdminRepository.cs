using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Entities;
using Dansnom.Entities.Identity;

namespace Dansnom.Interface.Repositories
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        Task<List<UserRole>> GetAllAdminsAsync();
        Task<Admin> GetAdminByUserIdAsync(int id);
        Task<UserRole> GetAdminByRoleAsync(string role);
        Task<Admin> GetAdminByEmailAsync(string email);
    }
    
}