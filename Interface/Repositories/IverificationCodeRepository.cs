using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Entities.Identity;

namespace Dansnom.Interface.Repositories
{
    public interface IVerificationCodeRepository : IBaseRepository<VerificationCode>
    {
        
    }
}