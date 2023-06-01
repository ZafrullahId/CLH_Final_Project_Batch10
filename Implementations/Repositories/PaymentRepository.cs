using Dansnom.Context;
using Dansnom.Entities;
using Dansnom.Implementations.Repositories;
using Interface.Repositories;

namespace Implementations.Repositories
{
    public class PaymentRepository : BaseRepository<PaymentReference>, IPaymentRepository
    {
        public PaymentRepository(DansnomApplicationContext context)
        {
            _Context = context;   
        }
    }
}