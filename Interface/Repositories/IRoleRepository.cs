using System.Threading.Tasks;
using Dansnom.Entities;
using Dansnom.Entities.Identity;

namespace Dansnom.Interface.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetRoleByUserId(int id);
    }
    
}