using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Dansnom.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Implementations.Repositories
{
    public class VarificationCodeRepository : BaseRepository<VerificationCode> , IverificationCodeRepository
    {
        public VarificationCodeRepository(DansnomApplicationContext context)
        {
            _Context = context;
        } 

    }
}