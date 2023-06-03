using System.Linq;
using System.Threading.Tasks;
using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Implementations.Repositories;
using Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Implementations.Repositories
{
    public class PaymentRepository : BaseRepository<PaymentReference>, IPaymentRepository
    {
        public PaymentRepository(DansnomApplicationContext context)
        {
            _Context = context;   
        }
        public async Task<PaymentReference> GetAsync(string transactionReference)
        {
            return await _Context.PaymentReferences
            .Include(x => x.Order)
            .Include(x => x.Customer)
            .ThenInclude(c => c.User)
            .Where(x => x.ReferenceNumber == transactionReference).SingleOrDefaultAsync();
        }
    }
}