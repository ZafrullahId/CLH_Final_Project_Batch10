using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Entities.Identity;

namespace Dansnom.Interface.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<UserRole> LoginAsync(string email, string password);
        Task<List<UserRole>> GetUserByRoleAsync(string role);
    }
    
}